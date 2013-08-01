namespace Labo.DownloadManager.Settings
{
    public sealed class InMemoryDownloadSettings : IDownloadSettings
    {
        private readonly int m_MinimumSegmentSize;
        private readonly int m_MaximumSegmentCount;
        private readonly int m_DownloadBufferSize;
        private readonly int m_MaximumConcurrentDownloads;

        public int MinimumSegmentSize
        {
            get { return m_MinimumSegmentSize; }
        }

        public int MaximumSegmentCount
        {
            get { return m_MaximumSegmentCount; }
        }

        public int DownloadBufferSize
        {
            get { return m_DownloadBufferSize; }
        }

        public int MaximumConcurrentDownloads
        {
            get { return m_MaximumConcurrentDownloads; }
        }

        public InMemoryDownloadSettings(int minimumSegmentSize, int maximumSegmentCount, int downloadBufferSize, int maximumConcurrentDownloads)
        {
            m_MinimumSegmentSize = minimumSegmentSize;
            m_MaximumSegmentCount = maximumSegmentCount;
            m_DownloadBufferSize = downloadBufferSize;
            m_MaximumConcurrentDownloads = maximumConcurrentDownloads;
        }
    }
}