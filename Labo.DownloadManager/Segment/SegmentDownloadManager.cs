namespace Labo.DownloadManager.Segment
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    public sealed class SegmentDownloadManager
    {
        private readonly Queue<ISegmentDownloadTask> m_Downloaders;
        private readonly object m_Locker = new object();
        private readonly Thread[] m_Workers;

        public SegmentDownloadManager(SegmentDownloadTaskCollection downloaders)
        {
            if (downloaders == null)
            {
                throw new ArgumentNullException("downloaders");
            }

            m_Downloaders = new Queue<ISegmentDownloadTask>(downloaders);
            m_Workers = new Thread[downloaders.Count];

            for (int i = 0; i < downloaders.Count; i++)
            {
                m_Workers[i] = new Thread(DoDownload);
            }
        }

        public void EnqueueDownloader(ISegmentDownloadTask segmentDownloader)
        {
            lock (m_Locker)
            {
                m_Downloaders.Enqueue(segmentDownloader);
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
                ISegmentDownloadTask downloader;
                lock (m_Locker)
                {
                    while (m_Downloaders.Count == 0)
                    {
                        Monitor.Wait(m_Locker);
                    }

                    downloader = m_Downloaders.Dequeue();
                }

                if (downloader == null)
                {
                    return;
                }

                downloader.Download();
            }
        }
    }
}
