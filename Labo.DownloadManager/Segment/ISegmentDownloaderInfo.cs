namespace Labo.DownloadManager.Segment
{
    using System;

    /// <summary>
    /// The segment downloader info interface.
    /// </summary>
    public interface ISegmentDownloaderInfo
    {
        /// <summary>
        /// Gets the current position of the segment downloader.
        /// </summary>
        long CurrentPosition { get; }

        /// <summary>
        /// Gets the remaining transfer bytes count of the segment.
        /// </summary>
        long RemainingTransfer { get; }

        /// <summary>
        /// Gets the downloaded bytes count of the segment.
        /// </summary>
        long TransferedDownload { get; }

        /// <summary>
        /// Gets the remaining download time.
        /// </summary>
        TimeSpan? RemainingTime { get; }

        /// <summary>
        /// Gets a value indicating whether [is download finished].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is download finished]; otherwise, <c>false</c>.
        /// </value>
        bool IsDownloadFinished { get; }

        /// <summary>
        /// Gets the download rate.
        /// </summary>
        /// <value>
        /// The download rate.
        /// </value>
        double? DownloadRate { get; }

        /// <summary>
        /// Gets the segment download progress.
        /// </summary>
        /// <value>
        /// The download progress.
        /// </value>
        double DownloadProgress { get; }

        /// <summary>
        /// Gets the start position of the download segment.
        /// </summary>
        /// <value>
        /// The start position.
        /// </value>
        long StartPosition { get; }

        /// <summary>
        /// Gets the end position of the download segment.
        /// </summary>
        /// <value>
        /// The end position.
        /// </value>
        long EndPosition { get; }

        /// <summary>
        /// Gets the state of the download segment.
        /// </summary>
        /// <value>
        /// The download segment state.
        /// </value>
        SegmentState State { get; }

        /// <summary>
        /// Gets the last exception.
        /// </summary>
        /// <value>
        /// The last exception.
        /// </value>
        Exception LastException { get; }

        /// <summary>
        /// Gets the last exception time.
        /// </summary>
        /// <value>
        /// The last exception time.
        /// </value>
        DateTime? LastExceptionTime { get; }

        /// <summary>
        /// Gets the download URI of the segment.
        /// </summary>
        /// <value>
        /// The URI.
        /// </value>
        Uri Uri { get; }

        /// <summary>
        /// Gets the download finish date.
        /// </summary>
        /// <value>
        /// The download finish date.
        /// </value>
        DateTime? DownloadFinishDate { get; }
    }
}