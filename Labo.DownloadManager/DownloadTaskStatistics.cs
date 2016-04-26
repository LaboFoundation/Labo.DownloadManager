namespace Labo.DownloadManager
{
    using System;

    using Labo.DownloadManager.Segment;

    /// <summary>
    /// The download task statistics.
    /// </summary>
    public sealed class DownloadTaskStatistics
    {
        /// <summary>
        /// The download file segments
        /// </summary>
        private SegmentDownloaderInfoCollection m_Segments;

        /// <summary>
        /// Gets or sets the download file URI.
        /// </summary>
        /// <value>
        /// The download file URI.
        /// </value>
        public Uri FileUri { get; set; }

        /// <summary>
        /// Gets or sets the size of the download file.
        /// </summary>
        /// <value>
        /// The size of the file.
        /// </value>
        public long FileSize { get; set; }

        /// <summary>
        /// Gets or sets the download status message.
        /// </summary>
        /// <value>
        /// The download status message.
        /// </value>
        public string StatusMessage { get; set; }

        /// <summary>
        /// Gets or sets the last error.
        /// </summary>
        /// <value>
        /// The last error.
        /// </value>
        public string LastError { get; set; }

        /// <summary>
        /// Gets or sets the remaining bytes to be transferred.
        /// </summary>
        /// <value>
        /// The remaining bytes to be transferred.
        /// </value>
        public long RemainingTransfer { get; set; }

        /// <summary>
        /// Gets or sets the transferred download.
        /// </summary>
        /// <value>
        /// The transferred download.
        /// </value>
        public long TransferedDownload { get; set; }

        /// <summary>
        /// Gets or sets the remaining download time.
        /// </summary>
        /// <value>
        /// The remaining download time.
        /// </value>
        public TimeSpan? RemainingTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is download resumable].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is download resumable]; otherwise, <c>false</c>.
        /// </value>
        public bool IsDownloadResumable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [is download finished].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is download finished]; otherwise, <c>false</c>.
        /// </value>
        public bool IsDownloadFinished { get; set; }

        /// <summary>
        /// Gets or sets the download rate.
        /// </summary>
        /// <value>
        /// The download rate.
        /// </value>
        public double? DownloadRate { get; set; }

        /// <summary>
        /// Gets or sets the average download rate.
        /// </summary>
        /// <value>
        /// The average download rate.
        /// </value>
        public double? AverageDownloadRate { get; set; }

        /// <summary>
        /// Gets or sets the download progress.
        /// </summary>
        /// <value>
        /// The download progress.
        /// </value>
        public double DownloadProgress { get; set; }

        /// <summary>
        /// Gets or sets the download task created date.
        /// </summary>
        /// <value>
        /// The download task created date.
        /// </value>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Gets or sets the state of the download task.
        /// </summary>
        /// <value>
        /// The state of the download task.
        /// </value>
        public DownloadTaskState DownloadTaskState { get; set; }

        /// <summary>
        /// Gets or sets the download segments.
        /// </summary>
        /// <value>
        /// The download segments.
        /// </value>
        public SegmentDownloaderInfoCollection Segments
        {
            get
            {
                return m_Segments ?? (m_Segments = new SegmentDownloaderInfoCollection());
            }

            set
            {
                m_Segments = value;
            }
        }

        /// <summary>
        /// Gets or sets the download finish date.
        /// </summary>
        /// <value>
        /// The download finish date.
        /// </value>
        public DateTime? DownloadFinishDate { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the download location.
        /// </summary>
        /// <value>
        /// The download location.
        /// </value>
        public string DownloadLocation { get; set; }

        /// <summary>
        /// Gets or sets the GUID.
        /// </summary>
        /// <value>
        /// The GUID.
        /// </value>
        public Guid Guid { get; set; }
    }
}