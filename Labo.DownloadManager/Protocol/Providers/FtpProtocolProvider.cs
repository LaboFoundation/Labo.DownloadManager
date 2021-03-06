﻿using System.IO;
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

        public RemoteFileInfo GetRemoteFileInfo(DownloadFileRequestInfo file)
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
            }

            return remoteFileInfo;
        }

        public Stream CreateStream(DownloadFileRequestInfo file, long startPosition, long endPosition)
        {
            FtpWebRequest request = (FtpWebRequest)m_WebRequestManager.GetWebRequest(file);

            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.ContentOffset = startPosition;

            WebResponse response = request.GetResponse();

            return response.GetResponseStream();
        }
    }
}
