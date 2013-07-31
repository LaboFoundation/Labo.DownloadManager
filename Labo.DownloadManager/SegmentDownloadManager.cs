using System.Collections.Generic;
using System.Threading;

namespace Labo.DownloadManager
{
    public sealed class SegmentDownloadManager
    {
        private readonly Queue<ISegmentDownloadTask> m_Downloaders;
        private readonly object m_Locker = new object();
        private readonly Thread[] m_Workers;

        public SegmentDownloadManager(ICollection<ISegmentDownloadTask> downloaders)
        {
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

            for (int i = 0; i < m_Workers.Length; i++)
            {
                Thread worker = m_Workers[i];
                worker.Join();
            }
        }

        private void DoDownload()
        {
            while (true)
            {
                ISegmentDownloadTask downloader;
                lock (m_Locker)
                {
                    if (m_Downloaders.Count == 0)
                    {
                        return;
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
