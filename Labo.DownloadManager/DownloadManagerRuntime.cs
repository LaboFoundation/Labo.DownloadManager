using Labo.DownloadManager.EventAggregator;
using Labo.DownloadManager.Protocol;
using Labo.DownloadManager.Segment;

namespace Labo.DownloadManager
{
    public static class DownloadManagerRuntime
    {
        private static readonly IUrlProtocolParser s_UrlProtocolParser = new UrlProtocolParser();
        private static readonly INetworkProtocolProviderFactory s_NetworkProtocolProviderFactory = new NetworkProtocolProviderFactory(s_UrlProtocolParser);
        private static readonly IDownloadSegmentPositionsCalculator s_DownloadSegmentPositionsCalculator = new DownloadSegmentPositionsCalculator();
        private static readonly IFileNameCorrector s_FileNameCorrector = new DefaultFileNameCorrector();        
        private static readonly ILocalFileAllocator s_LocalFileAllocator = new TempFileAllocator(s_FileNameCorrector);
        private static readonly IEventManager s_EventManager = new EventManager();

        public static INetworkProtocolProviderFactory NetworkProtocolProviderFactory
        {
            get { return s_NetworkProtocolProviderFactory; }
        }

        public static IDownloadSegmentPositionsCalculator DownloadSegmentPositionsCalculator
        {
            get { return s_DownloadSegmentPositionsCalculator; }
        }

        public static ILocalFileAllocator LocalFileAllocator
        {
            get { return s_LocalFileAllocator; }
        }

        public static IEventManager EventManager
        {
            get { return s_EventManager; }
        }
    }
}
