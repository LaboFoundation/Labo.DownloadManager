namespace Labo.DownloadManager
{
    public interface IDownloadTask
    {
        void StartDownload();

        bool IsWorking();

        void ChangeState(DownloadTaskState downloadTaskState);

        DownloadTaskState State { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate")]
        DownloadTaskStatistics GetDownloadTaskStatistics();
    }
}