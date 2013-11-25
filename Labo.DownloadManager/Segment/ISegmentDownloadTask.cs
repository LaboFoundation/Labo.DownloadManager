namespace Labo.DownloadManager.Segment
{
    public interface ISegmentDownloadTask
    {
        void Download();

        ISegmentDownloaderInfo SegmentDownloaderInfo { get; }
    }
}