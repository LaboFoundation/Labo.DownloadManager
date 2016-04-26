namespace Labo.DownloadManager
{
    public interface IDownloadTaskQueue
    {
        void EnqueueDownloadTask(IDownloadTask downloadTask);

        void Start();

        void Shutdown(bool waitForDownloads);

        int ActiveDownloadsCount { get; }

        int WaitingDownloadsCount { get; }
    }
}