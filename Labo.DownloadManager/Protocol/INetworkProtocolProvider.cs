using System.IO;

namespace Labo.DownloadManager.Protocol
{
    public interface INetworkProtocolProvider
    {
        RemoteFileInfo GetRemoteFileInfo(DownloadFile file);

        Stream CreateStream(DownloadFile file, long startPosition, long endPosition);
    }
}