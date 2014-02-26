namespace Labo.DownloadManager.Segment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Serializable]
    public sealed class SegmentDownloadTaskCollection : List<ISegmentDownloadTask>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentDownloadTaskCollection"/> class.
        /// </summary>
        public SegmentDownloadTaskCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentDownloadTaskCollection"/> class.
        /// </summary>
        /// <param name="collection">The collection.</param>
        public SegmentDownloadTaskCollection(IEnumerable<ISegmentDownloadTask> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentDownloadTaskCollection"/> class.
        /// </summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        public SegmentDownloadTaskCollection(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// Gets the download task statistics.
        /// </summary>
        /// <returns>The download task statistics.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public DownloadTaskStatistics GetDownloadTaskStatistics()
        {
            double downloadRate = 0;
            bool isDownloadFinished = Count != 0;
            long remainingTransfer = 0;
            long transferedDownload = 0;
            DateTime? downloadFinishDate = null;
            
            for (int i = 0; i < Count; i++)
            {
                ISegmentDownloadTask segmentDownloadTask = this[i];
                ISegmentDownloaderInfo segmentDownloaderInfo = segmentDownloadTask.SegmentDownloaderInfo;
                downloadRate += segmentDownloaderInfo.DownloadRate.GetValueOrDefault(0);
                remainingTransfer += segmentDownloaderInfo.RemainingTransfer;
                transferedDownload += segmentDownloaderInfo.TransferedDownload;

                if (!segmentDownloaderInfo.IsDownloadFinished)
                {
                    isDownloadFinished = false;
                }
                else
                {
                    if (downloadFinishDate.HasValue)
                    {
                        downloadFinishDate = downloadFinishDate > segmentDownloaderInfo.DownloadFinishDate ? downloadFinishDate : segmentDownloaderInfo.DownloadFinishDate;
                    }
                    else
                    {
                        downloadFinishDate = segmentDownloaderInfo.DownloadFinishDate;
                    }
                }
            }

            TimeSpan remainingTime = Math.Abs(downloadRate) < 0.001D ? TimeSpan.MaxValue : TimeSpan.FromSeconds(remainingTransfer / downloadRate);
            return new DownloadTaskStatistics
                       {
                           DownloadRate = downloadRate,
                           IsDownloadFinished = isDownloadFinished,
                           DownloadFinishDate = downloadFinishDate,
                           RemainingTransfer = remainingTransfer,
                           RemainingTime = remainingTime,
                           TransferedDownload = transferedDownload,
                           Segments = new SegmentDownloaderInfoCollection(this.Select(x => x.SegmentDownloaderInfo))
                       };
        }
    }
}