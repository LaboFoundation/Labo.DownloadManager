namespace Labo.DownloadManager.Segment
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public sealed class SegmentDownloadManager
    {
        private readonly object m_Locker = new object();

        private Queue<ISegmentDownloadTask> m_DownloadersQueue;
        private Thread[] m_Workers;

        private readonly SegmentDownloadTaskCollection m_SegmentDownloadTaskCollection;

        public SegmentDownloadTaskCollection SegmentDownloadTasks
        {
            get
            {
                return m_SegmentDownloadTaskCollection;
            }
        }

        public SegmentDownloadManager(SegmentDownloadTaskCollection downloaders)
        {
            if (downloaders == null)
            {
                throw new ArgumentNullException("downloaders");
            }

            m_SegmentDownloadTaskCollection = downloaders;
        }

        public void EnqueueDownloader(ISegmentDownloadTask segmentDownloader)
        {
            lock (m_Locker)
            {
                m_DownloadersQueue.Enqueue(segmentDownloader);
                Monitor.Pulse(m_Locker);
            }
        }

        public void Start()
        {
            m_DownloadersQueue = new Queue<ISegmentDownloadTask>(SegmentDownloadTasks);
            m_Workers = new Thread[SegmentDownloadTasks.Count];

            for (int i = 0; i < SegmentDownloadTasks.Count; i++)
            {
                m_Workers[i] = new Thread(DoDownload);
            }

            for (int i = 0; i < m_Workers.Length; i++)
            {
                Thread worker = m_Workers[i];
                worker.Start();
            }
        }

        public void Pause()
        {
            SegmentDownloadTasks.PauseAll();

            Finish(true);
        }

        public void Finish(bool waitForDownloads)
        {
            for (int i = 0; i < m_Workers.Length; i++)
            {
                EnqueueDownloader(null);
            }

            if (waitForDownloads)
            {
                for (int i = 0; i < m_Workers.Length; i++)
                {
                    Thread worker = m_Workers[i];
                    worker.Join();
                }
            }
          
            for (int i = 0; i < m_Workers.Length; i++)
            {
                Thread worker = m_Workers[i];
                worker.Abort();
            }
        }

        private void DoDownload()
        {
            while (true)
            {
                try
                {
                    ISegmentDownloadTask downloader;
                    lock (m_Locker)
                    {
                        while (m_DownloadersQueue.Count == 0)
                        {
                            Monitor.Wait(m_Locker);
                        }

                        downloader = m_DownloadersQueue.Dequeue();
                    }

                    if (downloader == null)
                    {
                        return;
                    }

                    downloader.Download();
                }
                catch (Exception ex)
                {
                    ex.ToString();
                }
            }
        }
    }
}
