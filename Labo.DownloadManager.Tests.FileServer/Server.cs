using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.SelfHost;

namespace Labo.DownloadManager.Tests.FileServer
{
    internal sealed class Server
    {
        private sealed class ContentHandler : HttpMessageHandler
        {
            private readonly Logger m_Logger;
            private readonly byte[] m_Data;

            public ContentHandler(Logger logger)
            {
                StringBuilder dataBuilder = new StringBuilder();
                for (int i = 0; i < 200000; i++)
                {
                    dataBuilder.Append(i);
                    dataBuilder.Append(" ");
                }
                m_Logger = logger;
                m_Data = Encoding.UTF8.GetBytes(dataBuilder.ToString());
            }

            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                return Task<HttpResponseMessage>.Factory.StartNew(() =>
                    {
                        bool isHeadMethod = request.Method == HttpMethod.Head;
                        long fileLength = m_Data.Length;
                        ContentInfo contentInfo = GetContentInfoFromRequest(request, fileLength);
                        PartialReadFileStream stream = new PartialReadFileStream(new DownloadStream(isHeadMethod ? new byte[0] : m_Data, m_Logger), contentInfo.From, contentInfo.To);
                        HttpResponseMessage response = new HttpResponseMessage();
                        response.Content = new StreamContent(stream);
                        SetResponseHeaders(response, contentInfo, fileLength, Path.GetFileName(request.RequestUri.AbsoluteUri));
                        return response;
                    });
            }

            private static ContentInfo GetContentInfoFromRequest(HttpRequestMessage request, long entityLength)
            {
                ContentInfo result = new ContentInfo
                {
                    From = 0,
                    To = entityLength - 1,
                    IsPartial = false,
                    Length = entityLength
                };
                RangeHeaderValue rangeHeader = request.Headers.Range;
                if (rangeHeader != null && rangeHeader.Ranges.Count != 0)
                {
                    //we support only one range
                    if (rangeHeader.Ranges.Count > 1)
                    {
                        //we probably return other status code here
                        throw new HttpResponseException(HttpStatusCode.RequestedRangeNotSatisfiable);
                    }
                    RangeItemHeaderValue range = rangeHeader.Ranges.First();
                    if (range.From.HasValue && range.From < 0 || range.To.HasValue && range.To > entityLength - 1)
                    {
                        throw new HttpResponseException(HttpStatusCode.RequestedRangeNotSatisfiable);
                    }

                    result.From = range.From ?? 0;
                    result.To = range.To ?? entityLength - 1;
                    result.IsPartial = true;
                    result.Length = entityLength;
                    if (range.From.HasValue && range.To.HasValue)
                    {
                        result.Length = range.To.Value - range.From.Value + 1;
                    }
                    else if (range.From.HasValue)
                    {
                        result.Length = entityLength - range.From.Value + 1;
                    }
                    else if (range.To.HasValue)
                    {
                        result.Length = range.To.Value + 1;
                    }
                }

                return result;
            }

            private static void SetResponseHeaders(HttpResponseMessage response, ContentInfo contentInfo, long fileLength, string fileName)
            {
                response.Headers.AcceptRanges.Add("bytes");
                response.StatusCode = contentInfo.IsPartial ? HttpStatusCode.PartialContent : HttpStatusCode.OK;
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
                response.Content.Headers.ContentDisposition.FileName = fileName;
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(MimeType.GetType(Path.GetExtension(fileName), "text/plain"));
                response.Content.Headers.ContentLength = contentInfo.Length;
                if (contentInfo.IsPartial)
                {
                    response.Content.Headers.ContentRange = new ContentRangeHeaderValue(contentInfo.From, contentInfo.To, fileLength);
                }
            }
        }

        private HttpSelfHostServer m_SelfHostServer;
        private readonly Logger m_Logger;

        public Server(Logger logger)
        {
            m_Logger = logger;
        }

        public bool Started { get; private set; }

        public void Start(string baseAddress)
        {
            HttpSelfHostConfiguration httpSelfHostConfiguration = new HttpSelfHostConfiguration(baseAddress);
            httpSelfHostConfiguration.TransferMode = TransferMode.Streamed;
            m_SelfHostServer = new HttpSelfHostServer(httpSelfHostConfiguration, new ContentHandler(m_Logger));

            Task task = m_SelfHostServer.OpenAsync();
            task.Wait();

            Started = true;
            m_Logger.Log(string.Format("Server started ({0})", baseAddress));
        }

        public void Stop()
        {
            Task task = m_SelfHostServer.CloseAsync();
            task.Wait();

            Started = false;

            m_Logger.Log("Server stopped");
        }
    }
}
