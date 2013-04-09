using System;

namespace Labo.DownloadManager
{
    public interface ISegmentDownloader
    {
        long CurrentPosition { get; }

        long RemainingTransfer { get; }

        long TransferedDownload { get; }

        TimeSpan? RemainingTime { get; }

        bool IsDownloadFinished { get; }

        double? DownloadRate { get; }

        double DownloadProgress { get; }

        long StartPosition { get; }

        long EndPosition { get; }

        int Download(byte[] buffer);

        void IncreaseCurrentPosition(int size);
    }
}