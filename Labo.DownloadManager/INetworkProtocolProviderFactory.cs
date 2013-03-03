using System.IO;

namespace Labo.DownloadManager
{
    public interface INetworkProtocolProviderFactory
    {
        INetworkProtocolProvider CreateProvider(string url);
    }
}