using System.IO;

namespace Labo.DownloadManager
{
    public interface INetworkProtocolProvider
    {
        RemoteFileInfo GetFileInfo(DownloadFile file);

        Stream CreateStream(DownloadFile file, long startPosition, long endPosition);
    }
}