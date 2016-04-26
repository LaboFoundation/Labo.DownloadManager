namespace Labo.DownloadManager
{
    using System;

    using Labo.Threading;

    public sealed class DownloadTaskQueue : IDownloadTaskQueue
    {
        private readonly WorkerThreadPool m_WorkerThreadPool;

        public int ActiveDownloadsCount
        {
            get
            {
                return m_WorkerThreadPool.CurrentWorkItemsCount - m_WorkerThreadPool.WorkItemQueueCount;
            }
        }

        public int WaitingDownloadsCount
        {
            get
            {
                return m_WorkerThreadPool.WorkItemQueueCount;
            }
        }

        public DownloadTaskQueue(int workerCount)
        {
            m_WorkerThreadPool = new WorkerThreadPool(60 * 1000, workerCount, workerCount);
        }

        public void ChangeWorkerCount(int workerCount)
        {
            if (workerCount < m_WorkerThreadPool.MinWorkerThreads)
            {
                m_WorkerThreadPool.SetMinimumWorkerThreadsCount(workerCount);
                m_WorkerThreadPool.SetMaximumWorkerThreadsCount(workerCount);
            }
            else
            {
                m_WorkerThreadPool.SetMaximumWorkerThreadsCount(workerCount);
                m_WorkerThreadPool.SetMinimumWorkerThreadsCount(workerCount);
            }
        }

        public void EnqueueDownloadTask(IDownloadTask downloadTask)
        {
            if (downloadTask == null)
            {
                throw new ArgumentNullException("downloadTask");
            }

            downloadTask.ChangeState(DownloadTaskState.Queued);
            m_WorkerThreadPool.QueueWorkItem(new ActionWorkItem(() => downloadTask.StartDownload(), downloadTask.PauseDownload));
        }

        public void Start()
        {
        }

        public void Shutdown(bool waitForDownloads)
        {
           m_WorkerThreadPool.Shutdown(waitForDownloads);
        }
    }
}