using System.Net;

namespace Labo.DownloadManager.Protocol
{
    public interface IWebRequestManager
    {
        WebRequest GetWebRequest(DownloadFileRequestInfo file);
    }
}