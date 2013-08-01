using System.IO;

namespace Labo.DownloadManager.EventArgs
{
    public sealed class DownloadTaskFinishedEventMessage
    {
        public DownloadTaskFinishedEventMessage(IDownloadTask downloadTask, Stream downloadStream, RemoteFileInfo remoteFileInfo)
        {
            DownloadTask = downloadTask;
            RemoteFileInfo = remoteFileInfo;
            DownloadStream = downloadStream;
        }

        public Stream DownloadStream { get; private set; }

        public RemoteFileInfo RemoteFileInfo { get; private set; }

        public IDownloadTask DownloadTask { get; private set; }
    }
}
