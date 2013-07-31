namespace Labo.DownloadManager
{
    public sealed class DownloadManager
    {
        private readonly IDownloadTaskList m_DownloadTaskList;
        private readonly DownloadTaskQueue m_DownloadTaskQueue;

        public DownloadManager()
        {
            m_DownloadTaskList = new DownloadTaskList();
            m_DownloadTaskQueue = new DownloadTaskQueue(5);
        }

        public void AddNewDownloadTask(DownloadFileInfo downloadFileInfo)
        {
            AddNewDownloadTask(new DownloadTask(DownloadManagerRuntime.NetworkProtocolProviderFactory,
                                                DownloadManagerRuntime.DownloadSegmentPositionsCalculator,
                                                DownloadManagerRuntime.LocalFileAllocator,
                                                DownloadManagerRuntime.EventManager, downloadFileInfo));
        }

        internal void AddNewDownloadTask(IDownloadTask downloadTask)
        {
            m_DownloadTaskList.Add(downloadTask);

            if (downloadTask.IsWorking())
            {
                m_DownloadTaskQueue.EnqueueDownloadTask(downloadTask);
            }
        }
    }
}
