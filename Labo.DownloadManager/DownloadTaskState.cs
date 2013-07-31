namespace Labo.DownloadManager
{
    public enum DownloadTaskState
    {
        Stopped = 0,
        Queued,
        Preparing,
        WaitingForReconnect,
        Prepared,
        Working,
        Pausing,
        Paused,
        Ended,
        EndedWithError
    }
}