using System.IO;

namespace Labo.DownloadManager.EventArgs
{
    public sealed class DownloadTaskFinishedEventArgs
    {
        public DownloadTaskFinishedEventArgs(Stream downloadStream, RemoteFileInfo remoteFileInfo)
        {
            RemoteFileInfo = remoteFileInfo;
            DownloadStream = downloadStream;
        }

        public Stream DownloadStream { get; private set; }

        public RemoteFileInfo RemoteFileInfo { get; private set; }
    }
}
