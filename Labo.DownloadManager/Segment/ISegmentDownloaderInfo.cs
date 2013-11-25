namespace Labo.DownloadManager.Segment
{
    using System;

    public interface ISegmentDownloaderInfo
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

        SegmentState State { get; }

        Exception LastException { get; }

        DateTime? LastExceptionTime { get; }

        string Url { get; }
    }
}