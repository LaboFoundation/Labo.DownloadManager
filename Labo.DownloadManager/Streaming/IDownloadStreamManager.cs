using System.IO;

namespace Labo.DownloadManager.Streaming
{
    public interface IDownloadStreamManager
    {
        Stream CreateStream(RemoteFileInfo remoteFileInfo);
    }
}