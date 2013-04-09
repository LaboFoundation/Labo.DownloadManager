using System.Threading;

namespace Labo.DownloadManager.Tests
{
    public sealed class SegmentDownloaderSimulator : SegmentDownloaderBase
    {
        private readonly ISegmentDownloader m_SegmentDownloader;

        public override long CurrentPosition
        {
            get { return m_SegmentDownloader.CurrentPosition; }
        }

        public override double? DownloadRate
        {
            get { throw new System.NotImplementedException(); }
        }

        public override long StartPosition
        {
            get { return m_SegmentDownloader.StartPosition; }
        }

        public override long EndPosition
        {
            get { return m_SegmentDownloader.EndPosition; }
        }

        public override int Download(byte[] buffer)
        {
            Thread.Sleep(2000);
            return m_SegmentDownloader.Download(buffer);
        }

        public override void IncreaseCurrentPosition(int size)
        {
            m_SegmentDownloader.IncreaseCurrentPosition(size);
        }

        public SegmentDownloaderSimulator(ISegmentDownloader segmentDownloader)
        {
            m_SegmentDownloader = segmentDownloader;
        }
    }
}
