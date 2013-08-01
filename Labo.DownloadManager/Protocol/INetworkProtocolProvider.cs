using System.IO;

namespace Labo.DownloadManager.Protocol
{
    public interface INetworkProtocolProvider
    {
<<<<<<< HEAD
        RemoteFileInfo GetRemoteFileInfo(DownloadFileInfo file);
=======
        RemoteFileInfo GetRemoteFileInfo(DownloadFile file, out Stream stream);
>>>>>>> 6bb4df88966e7f0c08b9450197079385b2b1d098

        Stream CreateStream(DownloadFileInfo file, long startPosition, long endPosition);
    }
}