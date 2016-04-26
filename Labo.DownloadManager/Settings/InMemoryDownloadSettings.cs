namespace Labo.DownloadManager.Settings
{
    public sealed class InMemoryDownloadSettings : IDownloadSettings
    {
        private readonly int m_MinimumSegmentSize;
        private readonly int m_MaximumSegmentCount;
        private readonly int m_DownloadBufferSize;
        private readonly int m_MaximumConcurrentDownloads;
        private readonly int m_MaximumRetries;

        private readonly int m_RetryDelayTimeInMilliseconds;

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

        public int MaximumRetries
        {
            get { return m_MaximumRetries; }
        }

        public int RetryDelayTimeInMilliseconds
        {
            get
            {
                return m_RetryDelayTimeInMilliseconds;
            }
        }

        public InMemoryDownloadSettings(int minimumSegmentSize, int maximumSegmentCount, int downloadBufferSize, int maximumConcurrentDownloads, int maximumRetries, int retryDelayTimeInMilliseconds)
        {
            m_MinimumSegmentSize = minimumSegmentSize;
            m_MaximumSegmentCount = maximumSegmentCount;
            m_DownloadBufferSize = downloadBufferSize;
            m_MaximumConcurrentDownloads = maximumConcurrentDownloads;
            m_MaximumRetries = maximumRetries;
            m_RetryDelayTimeInMilliseconds = retryDelayTimeInMilliseconds;
        }
    }
}