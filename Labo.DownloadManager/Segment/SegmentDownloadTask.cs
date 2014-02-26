namespace Labo.DownloadManager.Segment
{
    using System;

    public sealed class SegmentDownloadTask : ISegmentDownloadTask
    {
        /// <summary>
        /// The segment downloader
        /// </summary>
        private readonly ISegmentDownloader m_SegmentDownloader;

        /// <summary>
        /// The segment writer
        /// </summary>
        private readonly ISegmentWriter m_SegmentWriter;

        /// <summary>
        /// The buffer size
        /// </summary>
        private readonly int m_BufferSize;

        /// <summary>
        /// Gets the segment downloader information.
        /// </summary>
        /// <value>
        /// The segment downloader information.
        /// </value>
        public ISegmentDownloaderInfo SegmentDownloaderInfo
        {
            get { return m_SegmentDownloader; }
        }

        /// <summary>
        /// Gets the download finish date.
        /// </summary>
        /// <value>
        /// The download finish date.
        /// </value>
        public DateTime? DownloadFinishDate { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentDownloadTask"/> class.
        /// </summary>
        /// <param name="bufferSize">Size of the buffer.</param>
        /// <param name="segmentDownloader">The segment downloader.</param>
        /// <param name="segmentWriter">The segment writer.</param>
        public SegmentDownloadTask(int bufferSize, ISegmentDownloader segmentDownloader, ISegmentWriter segmentWriter)
        {
            m_BufferSize = bufferSize;
            m_SegmentDownloader = segmentDownloader;
            m_SegmentWriter = segmentWriter;
        }

        /// <summary>
        /// Starts the segment download task.
        /// </summary>
        public void Download()
        {
            int readSize;
            byte[] buffer = new byte[m_BufferSize];

            do
            {
                readSize = m_SegmentDownloader.Download(buffer);

                lock (m_SegmentWriter)
                {
                    m_SegmentWriter.Write(m_SegmentDownloader.CurrentPosition, buffer, readSize);
                    m_SegmentDownloader.IncreaseCurrentPosition(readSize);

                    if (m_SegmentDownloader.IsDownloadFinished)
                    {
                        m_SegmentDownloader.SetDownloadFinishDate(DateTime.Now);
                        break;
                    }
                }
            }
            while (readSize > 0);
        }
    }
}
