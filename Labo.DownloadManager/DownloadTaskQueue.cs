namespace Labo.DownloadManager
{
    using System.Collections.Generic;
    using System.Threading;

    public sealed class DownloadTaskQueue : IDownloadTaskQueue
    {
        private readonly Queue<IDownloadTask> m_DownloadTasks;
        private readonly object m_Locker = new object();
        private readonly Thread[] m_Workers;

        public DownloadTaskQueue(int workerCount)
        {
            m_DownloadTasks = new Queue<IDownloadTask>();
            m_Workers = new Thread[workerCount];

            for (int i = 0; i < m_Workers.Length; i++)
            {
                m_Workers[i] = new Thread(DoDownload);
            }
        }

        public void EnqueueDownloadTask(IDownloadTask downloadTask)
        {
            lock (m_Locker)
            {
                m_DownloadTasks.Enqueue(downloadTask);

                if (downloadTask != null)
                {
                    downloadTask.ChangeState(DownloadTaskState.Queued);
                }

                Monitor.Pulse(m_Locker);
            }
        }

        public void Start()
        {
            for (int i = 0; i < m_Workers.Length; i++)
            {
                Thread worker = m_Workers[i];
                worker.Start();
            }
        }

        public void Shutdown(bool waitForDownloads)
        {
            for (int i = 0; i < m_Workers.Length; i++)
            {
                EnqueueDownloadTask(null);
            }

            if (waitForDownloads)
            {
                for (int i = 0; i < m_Workers.Length; i++)
                {
                    Thread worker = m_Workers[i];
                    if (worker != null)
                    {
                        worker.Join();                        
                    }
                }
            }

            for (int i = 0; i < m_Workers.Length; i++)
            {
                Thread worker = m_Workers[i];
                if (worker != null)
                {
                    worker.Abort();                    
                }

                m_Workers[i] = null;
            }
        }

        private void DoDownload()
        {
            while (true)
            {
                IDownloadTask downloadTask;
                lock (m_Locker)
                {
                    while (m_DownloadTasks.Count == 0)
                    {
                        Monitor.Wait(m_Locker);
                    }

                    downloadTask = m_DownloadTasks.Dequeue();
                }

                if (downloadTask == null)
                {
                    return;
                }

                downloadTask.StartDownload();
            }
        }
    }
}