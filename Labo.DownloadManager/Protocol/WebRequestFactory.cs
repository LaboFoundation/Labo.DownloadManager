using System;
using System.Net;

namespace Labo.DownloadManager.Protocol
{
    internal sealed class WebRequestFactory : IWebRequestFactory
    {
        public WebRequest CreateRequest(Uri uri)
        {
            return WebRequest.Create(uri);
        }
    }
}