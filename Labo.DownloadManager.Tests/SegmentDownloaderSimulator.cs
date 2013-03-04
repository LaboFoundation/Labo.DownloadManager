using System.Threading;

namespace Labo.DownloadManager.Tests
{
    public sealed class SegmentDownloaderSimulator : ISegmentDownloader
    {
        private readonly ISegmentDownloader m_SegmentDownloader;

        public long CurrentPosition
        {
            get { return m_SegmentDownloader.CurrentPosition; }
        }

        public bool IsDownloadFinished
        {
            get { return m_SegmentDownloader.IsDownloadFinished; }
        }

        public int Download(byte[] buffer)
        {
            Thread.Sleep(2000);
            return m_SegmentDownloader.Download(buffer);
        }

        public void IncreaseCurrentPosition(int size)
        {
            m_SegmentDownloader.IncreaseCurrentPosition(size);
        }

        public SegmentDownloaderSimulator(ISegmentDownloader segmentDownloader)
        {
            m_SegmentDownloader = segmentDownloader;
        }
    }
}
