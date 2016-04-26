using System.IO;

namespace Labo.DownloadManager.Protocol
{
    public interface INetworkProtocolProvider
    {
        RemoteFileInfo GetRemoteFileInfo(DownloadFileRequestInfo file);

        Stream CreateStream(DownloadFileRequestInfo file, long startPosition, long endPosition);
    }
}