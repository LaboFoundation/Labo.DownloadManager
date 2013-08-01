namespace Labo.DownloadManager.EventArgs
{
    public sealed class DownloadTaskStateChangedEventMessage
    {
        public DownloadTaskState State { get; private set; }

        public DownloadTaskStateChangedEventMessage(DownloadTaskState state)
        {
            State = state;
        }
    }
}
