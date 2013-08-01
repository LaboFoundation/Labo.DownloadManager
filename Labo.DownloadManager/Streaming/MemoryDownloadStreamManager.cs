using System.IO;

namespace Labo.DownloadManager.Streaming
{
    public sealed class MemoryDownloadStreamManager : IDownloadStreamManager
    {
        private readonly MemoryStream m_Stream;

        public MemoryDownloadStreamManager(MemoryStream stream)
        {
            m_Stream = stream;
        }

        public Stream CreateStream(RemoteFileInfo remoteFileInfo)
        {
            return m_Stream;
        }
    }
}