namespace Labo.DownloadManager.Segment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Serializable]
    public sealed class SegmentDownloadTaskCollection : List<ISegmentDownloadTask>
    {
        public SegmentDownloadTaskCollection()
            : base()
        {
        }

        public SegmentDownloadTaskCollection(IEnumerable<ISegmentDownloadTask> collection)
            : base(collection)
        {
        }

        public SegmentDownloadTaskCollection(int capacity)
            : base(capacity)
        {
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        public DownloadTaskStatistics GetDownloadTaskStatistics()
        {
            double downloadRate = 0;
            bool isDownloadFinished = Count != 0;
            long remainingTransfer = 0;
            long transferedDownload = 0;
            
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
            }

            TimeSpan remainingTime = downloadRate == 0D ? TimeSpan.MaxValue : TimeSpan.FromSeconds(remainingTransfer / downloadRate);
            return new DownloadTaskStatistics
                       {
                           DownloadRate = downloadRate,
                           IsDownloadFinished = isDownloadFinished,
                           RemainingTransfer = remainingTransfer,
                           RemainingTime = remainingTime,
                           TransferedDownload = transferedDownload,
                           Segments = new SegmentDownloaderInfoCollection(this.Select(x => x.SegmentDownloaderInfo))
                       };
        }
    }
}