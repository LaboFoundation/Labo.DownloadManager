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

        private static void SetCredentials(DownloadFile file, WebRequest webRequest)
        {
            if (file.Authenticate)
            {
                NetworkCredentialUserName networkCredentialUserName = new NetworkCredentialUserName(file.UserName);
                webRequest.Credentials = new NetworkCredential(networkCredentialUserName.UserName, file.Password, networkCredentialUserName.Domain);
            }
        }

        public WebRequest GetWebRequest(DownloadFile file)
        {
            WebRequest webRequest = m_WebRequestFactory.CreateRequest(new Uri(file.Url));
            webRequest.Timeout = 30000;

            SetCredentials(file, webRequest);

            return webRequest;
        }
    }
}