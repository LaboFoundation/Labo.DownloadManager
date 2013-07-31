namespace Labo.DownloadManager
{
    public interface IDownloadTask
    {
        void StartDownload();

        bool IsWorking();
        void ChangeState(DownloadTaskState downloadTaskState);
    }
}