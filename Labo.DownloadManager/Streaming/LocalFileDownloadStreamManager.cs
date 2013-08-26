using System;
using System.IO;

namespace Labo.DownloadManager.Streaming
{
    public sealed class LocalFileDownloadStreamManager : IDownloadStreamManager
    {
        private readonly ILocalFileAllocator m_LocalFileAllocator;

        public LocalFileDownloadStreamManager(ILocalFileAllocator localFileAllocator)
        {
            m_LocalFileAllocator = localFileAllocator;
        }

        public Stream CreateStream(RemoteFileInfo remoteFileInfo)
        {
            if (remoteFileInfo == null) throw new ArgumentNullException("remoteFileInfo");

            LocalFileInfo localFileInfo = m_LocalFileAllocator.AllocateFile(remoteFileInfo.FileName, remoteFileInfo.FileSize);
            return new FileStream(localFileInfo.FileName, FileMode.Open, FileAccess.Write);
        }
    }
}