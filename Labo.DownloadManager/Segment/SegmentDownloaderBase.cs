namespace Labo.DownloadManager.Segment
{
    using System;

    /// <summary>
    /// The segment downloader base class.
    /// </summary>
    public abstract class SegmentDownloaderBase : ISegmentDownloader
    {
        /// <summary>
        /// Gets the current position of the segment downloader.
        /// </summary>
        public abstract long CurrentPosition { get; }

        /// <summary>
        /// Gets the remaining transfer bytes count of the segment.
        /// </summary>
        public long RemainingTransfer
        {
            get
            {
                return EndPosition <= 0 ? 0 : EndPosition - CurrentPosition;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [is download finished].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is download finished]; otherwise, <c>false</c>.
        /// </value>
        public bool IsDownloadFinished
        {
            get { return EndPosition == -1 || CurrentPosition > EndPosition; }
        }

        /// <summary>
        /// Gets the download rate.
        /// </summary>
        /// <value>
        /// The download rate.
        /// </value>
        public abstract double? DownloadRate { get; }

        /// <summary>
        /// Gets the remaining download time.
        /// </summary>
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

        /// <summary>
        /// Gets the downloaded bytes count of the segment.
        /// </summary>
        public long TransferedDownload
        {
            get { return CurrentPosition - StartPosition; }
        }

        /// <summary>
        /// Gets the segment download progress.
        /// </summary>
        /// <value>
        /// The download progress.
        /// </value>
        public double DownloadProgress
        {
            get
            {
                return EndPosition <= 0 ? 0 : (TransferedDownload / (double)RemainingTransfer * 100.0);
            }
        }

        /// <summary>
        /// Gets the start position of the download segment.
        /// </summary>
        /// <value>
        /// The start position.
        /// </value>
        public abstract long StartPosition { get; }

        /// <summary>
        /// Gets the end position of the download segment.
        /// </summary>
        /// <value>
        /// The end position.
        /// </value>
        public abstract long EndPosition { get; }

        /// <summary>
        /// Reads a sequence of bytes from the current stream and advances 
        /// the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">
        /// An array of bytes that contains the specified byte array with the values which will be replaced 
        /// by the bytes read from the current source when the method returns.
        /// </param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if 
        /// that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        public abstract int Download(byte[] buffer);

        /// <summary>
        /// Increases the current position of the segment downloader.
        /// </summary>
        /// <param name="size">The increase size.</param>
        public abstract void IncreaseCurrentPosition(int size);

        /// <summary>
        /// Sets the error.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public abstract void SetError(Exception exception);

        /// <summary>
        /// Gets the state of the download segment.
        /// </summary>
        /// <value>
        /// The download segment state.
        /// </value>
        public abstract SegmentState State { get; }

        /// <summary>
        /// Gets the last exception.
        /// </summary>
        /// <value>
        /// The last exception.
        /// </value>
        public abstract Exception LastException { get; }

        /// <summary>
        /// Gets the last exception time.
        /// </summary>
        /// <value>
        /// The last exception time.
        /// </value>
        public abstract DateTime? LastExceptionTime { get; }

        /// <summary>
        /// Gets the download URI of the segment.
        /// </summary>
        /// <value>
        /// The URI.
        /// </value>
        public abstract Uri Uri { get; }

        /// <summary>
        /// Gets the download finish date.
        /// </summary>
        /// <value>
        /// The download finish date.
        /// </value>
        public DateTime? DownloadFinishDate { get; private set; }

        /// <summary>
        /// Sets the download finish date.
        /// </summary>
        /// <param name="date">The finish date.</param>
        public void SetDownloadFinishDate(DateTime date)
        {
            DownloadFinishDate = date;
        }
    }
}