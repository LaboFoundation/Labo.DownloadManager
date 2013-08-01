namespace Labo.DownloadManager.Settings
{
    public interface IDownloadSettings
    {
        int MinimumSegmentSize { get; }

        int MaximumSegmentCount { get; }

        int DownloadBufferSize { get; }

        int MaximumConcurrentDownloads { get; }
    }
}
