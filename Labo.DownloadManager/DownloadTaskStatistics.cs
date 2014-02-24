namespace Labo.DownloadManager
{
    using System;

    using Labo.DownloadManager.Segment;

    public sealed class DownloadTaskStatistics
    {
        private SegmentDownloaderInfoCollection m_Segments;

        public Uri FileUri { get; set; }

        public long FileSize { get; set; }

        public string StatusMessage { get; set; }

        public string LastError { get; set; }

        public long RemainingTransfer { get; set; }

        public long TransferedDownload { get; set; }

        public TimeSpan? RemainingTime { get; set; }

        public bool IsDownloadResumable { get; set; }

        public bool IsDownloadFinished { get; set; }

        public double? DownloadRate { get; set; }

        public double DownloadProgress { get; set; }

        public DateTime CreatedDate { get; set; }

        public DownloadTaskState DownloadTaskState { get; set; }

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
    }
}