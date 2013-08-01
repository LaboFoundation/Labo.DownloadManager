using System.IO;

namespace Labo.DownloadManager.Protocol
{
    public interface INetworkProtocolProvider
    {
        RemoteFileInfo GetRemoteFileInfo(DownloadFileInfo file);

        Stream CreateStream(DownloadFileInfo file, long startPosition, long endPosition);
    }
}