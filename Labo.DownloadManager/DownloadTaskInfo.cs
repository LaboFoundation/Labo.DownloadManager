namespace Labo.DownloadManager
{
    public sealed class DownloadTaskInfo
    {
        public DownloadTaskInfo(DownloadFileInfo downloadFileInfo, bool startImmediately)
        {
            StartImmediately = startImmediately;
            DownloadFileInfo = downloadFileInfo;
        }

        public DownloadFileInfo DownloadFileInfo { get; private set; }

        public bool StartImmediately { get; private set; }
    }
}