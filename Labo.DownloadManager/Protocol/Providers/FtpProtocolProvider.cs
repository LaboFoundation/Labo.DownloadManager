using System.IO;
using System.Net;

namespace Labo.DownloadManager.Protocol.Providers
{
    internal sealed class FtpProtocolProvider : INetworkProtocolProvider
    {
        private readonly IWebRequestManager m_WebRequestManager;

        public FtpProtocolProvider(IWebRequestManager webRequestManager)
        {
            m_WebRequestManager = webRequestManager;
        }

<<<<<<< HEAD
        public RemoteFileInfo GetRemoteFileInfo(DownloadFileInfo file)
=======
        public RemoteFileInfo GetRemoteFileInfo(DownloadFile file, out Stream stream)
>>>>>>> 6bb4df88966e7f0c08b9450197079385b2b1d098
        {
            RemoteFileInfo remoteFileInfo = new RemoteFileInfo();
            remoteFileInfo.AcceptRanges = true;

            FtpWebRequest request = (FtpWebRequest)m_WebRequestManager.GetWebRequest(file);
            request.Method = WebRequestMethods.Ftp.GetFileSize;

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                remoteFileInfo.FileSize = response.ContentLength;
            }

            request = (FtpWebRequest)m_WebRequestManager.GetWebRequest(file);
            request.Method = WebRequestMethods.Ftp.GetDateTimestamp;

            using (FtpWebResponse response = (FtpWebResponse)request.GetResponse())
            {
                remoteFileInfo.LastModified = response.LastModified;

                stream = response.GetResponseStream();
            }

            return remoteFileInfo;
        }

        public Stream CreateStream(DownloadFileInfo file, long startPosition, long endPosition)
        {
            FtpWebRequest request = (FtpWebRequest)m_WebRequestManager.GetWebRequest(file);

            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.ContentOffset = startPosition;

            WebResponse response = request.GetResponse();

            return response.GetResponseStream();
        }
    }
}
