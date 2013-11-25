using System;

namespace Labo.DownloadManager.Segment
{
    public abstract class SegmentDownloaderBase : ISegmentDownloader
    {
        public abstract long CurrentPosition { get; }

        public long RemainingTransfer
        {
            get
            {
                return EndPosition <= 0 ? 0 : EndPosition - CurrentPosition;
            }
        }

        public bool IsDownloadFinished
        {
            get { return EndPosition == -1 || CurrentPosition > EndPosition; }
        }

        public abstract double? DownloadRate { get; }

        public TimeSpan? RemainingTime
        {
            get
            {
                double? dowloadRate = DownloadRate;
                if (!dowloadRate.HasValue)
                {
                    return null;
                }

                if (dowloadRate.Value > 0.0)
                {
                    return TimeSpan.FromSeconds(RemainingTransfer / dowloadRate.Value);
                }

                return TimeSpan.MaxValue;
            }
        }

        public long TransferedDownload
        {
            get { return CurrentPosition - StartPosition; }
        }

        public double DownloadProgress
        {
            get
            {
                return EndPosition <= 0 ? 0 : (TransferedDownload / (double)RemainingTransfer * 100.0);
            }
        }

        public abstract long StartPosition { get; }

        public abstract long EndPosition { get; }

        public abstract int Download(byte[] buffer);

        public abstract void IncreaseCurrentPosition(int size);

        public abstract void SetError(Exception exception);

        public abstract SegmentState State { get; }

        public abstract Exception LastException { get; }

        public abstract DateTime? LastExceptionTime { get; }

        public abstract string Url { get; }
    }
}