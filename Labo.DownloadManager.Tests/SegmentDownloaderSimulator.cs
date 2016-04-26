using System.Threading;
using Labo.DownloadManager.Segment;

namespace Labo.DownloadManager.Tests
{
    using System;

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
            protected set { throw new System.NotImplementedException(); }
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

        public override void SetError(System.Exception exception)
        {
            throw new System.NotImplementedException();
        }

        public override SegmentState State
        {
            get { throw new System.NotImplementedException(); }
            set { throw new System.NotImplementedException(); }
        }

        public override System.Exception LastException
        {
            get { throw new System.NotImplementedException(); }
        }

        public override System.DateTime? LastExceptionTime
        {
            get { throw new System.NotImplementedException(); }
        }

        public override Uri Uri
        {
            get { throw new System.NotImplementedException(); }
        }

        public override void RefreshDownloadStream()
        {
            throw new NotImplementedException();
        }

        public override void CalculateCurrentDownloadRate(long currentPosition)
        {
            throw new NotImplementedException();
        }
    }
}
