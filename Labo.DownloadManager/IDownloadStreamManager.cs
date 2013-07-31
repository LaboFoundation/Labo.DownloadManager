using System.IO;

namespace Labo.DownloadManager
{
    public interface IDownloadStreamManager
    {
        Stream CreateStream(RemoteFileInfo remoteFileInfo);
    }
}