namespace Labo.DownloadManager.Protocol
{
    public interface INetworkProtocolProviderFactory
    {
        INetworkProtocolProvider CreateProvider(string url);
        void RegisterProvider(string protocol, INetworkProtocolProvider provider);
    }
}