namespace Labo.DownloadManager.Segment
{
    using System;

    public interface ISegmentDownloader : ISegmentDownloaderInfo
    {
        int Download(byte[] buffer);

        void IncreaseCurrentPosition(int size);

        void SetError(Exception exception);
    }
}