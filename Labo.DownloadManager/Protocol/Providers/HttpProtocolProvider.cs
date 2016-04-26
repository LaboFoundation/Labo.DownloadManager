namespace Labo.DownloadManager.Protocol.Providers
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Http.Headers;
    using System.Text;

    internal sealed class HttpProtocolProvider : INetworkProtocolProvider
    {
        private readonly IWebRequestManager m_WebRequestManager;

        static HttpProtocolProvider()
        {
            ServicePointManager.DefaultConnectionLimit = int.MaxValue;
        }

        public HttpProtocolProvider(IWebRequestManager webRequestManager)
        {
            m_WebRequestManager = webRequestManager;
        }

        public RemoteFileInfo GetRemoteFileInfo(DownloadFileRequestInfo file)
        {
            RemoteFileInfo remoteFileInfo = new RemoteFileInfo();
            string supportedMethod = GetSupportedMethod(file);

            WebRequest webRequest = m_WebRequestManager.GetWebRequest(file);
            webRequest.Method = supportedMethod;

            HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();
            remoteFileInfo.LastModified = httpWebResponse.LastModified;
            remoteFileInfo.MimeType = httpWebResponse.ContentType;
            remoteFileInfo.FileSize = httpWebResponse.ContentLength;
            remoteFileInfo.AcceptRanges = string.Compare(httpWebResponse.Headers["Accept-Ranges"], "bytes", StringComparison.OrdinalIgnoreCase) == 0;

            ContentDispositionHeaderValue contentDispositionHeaderValue;
            string contentDispositionString = DecodeUtf8FromString(httpWebResponse.Headers["Content-Disposition"]);

            if (ContentDispositionHeaderValue.TryParse(contentDispositionString, out contentDispositionHeaderValue))
            {
                string fileName = contentDispositionHeaderValue.FileName;
                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    remoteFileInfo.FileName = fileName.Trim('"');                    
                }
            }

            return remoteFileInfo;
        }

        private string GetSupportedMethod(DownloadFileRequestInfo file)
        {
            return "HEAD";

            string supportedMethod;
            WebRequest webRequest = m_WebRequestManager.GetWebRequest(file);
            webRequest.Method = "OPTIONS";

            HttpWebResponse httpWebResponse = (HttpWebResponse)webRequest.GetResponse();

            string accept = httpWebResponse.Headers.Get("Accept");
            if (!string.IsNullOrWhiteSpace(accept) && accept.IndexOf("head", StringComparison.OrdinalIgnoreCase) > -1)
            {
                supportedMethod = "HEAD";
            }
            else
            {
                supportedMethod = "GET";
            }
            return supportedMethod;
        }

        public Stream CreateStream(DownloadFileRequestInfo file, long startPosition, long endPosition)
        {
            lock (typeof(HttpWebRequest))
            {
                HttpWebRequest request = (HttpWebRequest)m_WebRequestManager.GetWebRequest(file);
                request.AddRange(startPosition, endPosition);

                WebResponse response = request.GetResponse();

                return response.GetResponseStream();
            }
        }

        internal static string DecodeUtf8FromString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            bool shouldDecode = false;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] > 'ÿ')
                {
                    return input;
                }

                if (input[i] > '\u007f')
                {
                    shouldDecode = true;
                    break;
                }
            }

            if (shouldDecode)
            {
                byte[] array = new byte[input.Length];
                for (int j = 0; j < input.Length; j++)
                {
                    array[j] = (byte)input[j];
                }

                try
                {
                    return Encoding.GetEncoding("utf-8", EncoderFallback.ExceptionFallback, DecoderFallback.ExceptionFallback).GetString(array);
                }
                catch (ArgumentException)
                {
                }

                return input;
            }

            return input;
        }
    }
}