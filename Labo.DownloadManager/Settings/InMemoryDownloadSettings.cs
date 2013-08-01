namespace Labo.DownloadManager.Settings
{
    public sealed class InMemoryDownloadSettings : IDownloadSettings
    {
        private readonly int m_MinimumSegmentSize;
        private readonly int m_MaximumSegmentCount;
        private readonly int m_DownloadBufferSize;

        public InMemoryDownloadSettings(int minimumSegmentSize, int maximumSegmentCount, int downloadBufferSize)
        {
            m_MinimumSegmentSize = minimumSegmentSize;
            m_MaximumSegmentCount = maximumSegmentCount;
            m_DownloadBufferSize = downloadBufferSize;
        }

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
    }
}