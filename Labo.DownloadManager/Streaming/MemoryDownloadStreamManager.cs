using System.IO;

namespace Labo.DownloadManager.Streaming
{
    public sealed class MemoryDownloadStreamManager : IDownloadStreamManager
    {
        public Stream CreateStream(RemoteFileInfo remoteFileInfo)
        {
            return new MemoryStream();
        }
    }
}