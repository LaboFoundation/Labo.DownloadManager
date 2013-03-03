namespace Labo.DownloadManager
{
    public sealed class SegmentDownloadTask : ISegmentDownloadTask
    {
        private readonly ISegmentDownloader m_SegmentDownloader;
        private readonly ISegmentWriter m_SegmentWriter;

        public SegmentDownloadTask(ISegmentDownloader segmentDownloader, ISegmentWriter segmentWriter)
        {
            m_SegmentDownloader = segmentDownloader;
            m_SegmentWriter = segmentWriter;
        }

        public void Download()
        {
            int readSize;
            byte[] buffer = new byte[8192];

            do
            {
                readSize = m_SegmentDownloader.Download(buffer);

                lock (m_SegmentWriter)
                {
                    m_SegmentWriter.Write(m_SegmentDownloader.CurrentPosition, buffer, readSize);
                    m_SegmentDownloader.IncreaseCurrentPosition(readSize);

                    if (m_SegmentDownloader.IsDownloadFinished)
                    {
                        break;
                    }
                }
            }
            while (readSize > 0);
        }
    }
}
