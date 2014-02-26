namespace Labo.DownloadManager.Segment
{
    public interface ISegmentDownloadTask
    {
        /// <summary>
        /// Starts the segment download task.
        /// </summary>
        void Download();

        /// <summary>
        /// Gets the segment downloader information.
        /// </summary>
        /// <value>
        /// The segment downloader information.
        /// </value>
        ISegmentDownloaderInfo SegmentDownloaderInfo { get; }
    }
}