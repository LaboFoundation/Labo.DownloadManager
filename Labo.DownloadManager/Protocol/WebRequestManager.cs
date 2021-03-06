using System;
using System.Net;

namespace Labo.DownloadManager.Protocol
{
    internal sealed class WebRequestManager : IWebRequestManager
    {
        private readonly IWebRequestFactory m_WebRequestFactory;

        public WebRequestManager(IWebRequestFactory webRequestFactory)
        {
            m_WebRequestFactory = webRequestFactory;
        }

        private static void SetCredentials(DownloadFileRequestInfo file, WebRequest webRequest)
        {
            if (file.Authenticate)
            {
                NetworkCredentialUserName networkCredentialUserName = new NetworkCredentialUserName(file.UserName);
                webRequest.Credentials = new NetworkCredential(networkCredentialUserName.UserName, file.Password, networkCredentialUserName.Domain);
            }
        }

        public WebRequest GetWebRequest(DownloadFileRequestInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException("file");
            }

            WebRequest webRequest = m_WebRequestFactory.CreateRequest(file.Uri);
            //webRequest.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF8");
            webRequest.Timeout = 30000;

            SetCredentials(file, webRequest);

            return webRequest;
        }
    }
}