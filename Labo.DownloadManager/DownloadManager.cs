using Labo.DownloadManager.Settings;

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
                                                new LocalFileDownloadStreamManager(new TempFileAllocator(new DefaultFileNameCorrector())),
                                                new MemoryDownloadSettings(200, 5, 8096), 
                                                downloadFileInfo, DownloadManagerRuntime.EventManager));
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
