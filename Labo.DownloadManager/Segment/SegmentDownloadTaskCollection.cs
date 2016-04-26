namespace Labo.DownloadManager.Segment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Serializable]
    public sealed class SegmentDownloadTaskCollection : List<ISegmentDownloadTask>
    {
        /// <summary>
        /// Yeni bir <see cref="SegmentDownloadTaskCollection"/> sınıfı örneği yaratır.
        /// </summary>
        public SegmentDownloadTaskCollection()
        {
        }

        /// <summary>
        /// Yeni bir <see cref="SegmentDownloadTaskCollection"/> sınıfı örneği yaratır.
        /// </summary>
        /// <param name="collection">Koleksiyon.</param>
        public SegmentDownloadTaskCollection(IEnumerable<ISegmentDownloadTask> collection)
            : base(collection)
        {
        }

        /// <summary>
        /// Yeni bir <see cref="SegmentDownloadTaskCollection"/> sınıfı örneği yaratır.
        /// </summary>
        /// <param name="capacity">Koleksiyonun ilk hacmi.</param>
        public SegmentDownloadTaskCollection(int capacity)
            : base(capacity)
        {
        }

        /// <summary>
        /// İndirme görevi istatistiklerini getirir.
        /// </summary>
        /// <returns>İndirme görevi istatistikleri.</returns>
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

        /// <summary>
        /// Bütün bölüt indirme görevlerini duraklatır.
        /// </summary>
        public void PauseAll()
        {
            for (int i = 0; i < Count; i++)
            {
                ISegmentDownloadTask segmentDownloadTask = this[i];
                segmentDownloadTask.Pause();
            }
        }
    }
}