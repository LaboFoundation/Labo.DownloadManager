using System;
using System.Net;

namespace Labo.DownloadManager.Protocol
{
    public interface IWebRequestFactory
    {
        WebRequest CreateRequest(Uri uri);
    }
}
