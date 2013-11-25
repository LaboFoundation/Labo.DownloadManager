namespace Labo.DownloadManager.Segment
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    internal sealed class DoubleBufferSegmentDownloadTask : ISegmentDownloadTask
    {
        private sealed class SegmentDownloadInfo
        {
            public int Size { get; set; }

            public long CurrentPosition { get; set; }

            public byte[] Buffer { get; set; }
        }

        private readonly int m_BufferSize;
        private readonly ISegmentDownloader m_SegmentDownloader;
        private readonly ISegmentWriter m_SegmentWriter;
        private readonly Thread[] m_Threads;

        private readonly Queue<byte[]> m_InputBufferQueue;
        private readonly Queue<SegmentDownloadInfo> m_OutputBufferQueue;
        private readonly object m_SegmentDownloaderLocker = new object();
        private readonly object m_SegmentWriterLocker = new object();

        public DoubleBufferSegmentDownloadTask(int bufferSize, ISegmentDownloader segmentDownloader, ISegmentWriter segmentWriter)
        {
            m_BufferSize = bufferSize;
            m_SegmentDownloader = segmentDownloader;
            m_SegmentWriter = segmentWriter;
            m_InputBufferQueue = new Queue<byte[]>();
            m_OutputBufferQueue = new Queue<SegmentDownloadInfo>();
            m_Threads = new Thread[2];
        }

        public ISegmentDownloaderInfo SegmentDownloaderInfo
        {
            get
            {
                return m_SegmentDownloader;
            }
        }

        public void Download()
        {
            EnqueueDownloadSegment(new byte[m_BufferSize]);
            EnqueueDownloadSegment(new byte[m_BufferSize]);

            (m_Threads[0] = new Thread(DownloadSegment)).Start();
            (m_Threads[1] = new Thread(WriteSegment)).Start();

            Shutdown(true);
        }

        public void Shutdown(bool waitAllThreads)
        {
            if (waitAllThreads)
            {
                for (int i = 0; i < m_Threads.Length; i++)
                {
                    Thread thread = m_Threads[i];
                    thread.Join();
                }
            }

            for (int i = 0; i < m_Threads.Length; i++)
            {
                Thread thread = m_Threads[i];
                thread.Abort();

                m_Threads[i] = null;
            }
        }

        private void DownloadSegment()
        {
            while (true)
            {
                try
                {
                    byte[] buffer;
                    lock (m_SegmentDownloaderLocker)
                    {
                        while (m_InputBufferQueue.Count == 0)
                        {
                            Monitor.Wait(m_SegmentDownloaderLocker);
                        }

                        buffer = m_InputBufferQueue.Dequeue();
                    }

                    if (buffer == null)
                    {
                        return;
                    }

                    lock (m_SegmentDownloader)
                    {
                        int size = m_SegmentDownloader.Download(buffer);

                        long currentPosition = m_SegmentDownloader.CurrentPosition;

                        m_SegmentDownloader.IncreaseCurrentPosition(size);

                        EnqueueWriteSegment(new SegmentDownloadInfo
                        {
                            Buffer = buffer,
                            CurrentPosition = currentPosition,
                            Size = size
                        });

                        if (m_SegmentDownloader.IsDownloadFinished)
                        {
                            EnqueueWriteSegment(null);
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    m_SegmentDownloader.SetError(ex);

                    EnqueueWriteSegment(null);
                    return;
                }
            }
        }

        private void EnqueueWriteSegment(SegmentDownloadInfo segmentDownloadInfo)
        {
            lock (m_SegmentWriterLocker)
            {
                m_OutputBufferQueue.Enqueue(segmentDownloadInfo);
                Monitor.Pulse(m_SegmentWriterLocker);
            }
        }

        private void EnqueueDownloadSegment(byte[] buffer)
        {
            lock (m_SegmentDownloaderLocker)
            {
                m_InputBufferQueue.Enqueue(buffer);
                Monitor.Pulse(m_SegmentDownloaderLocker);
            }
        }

        private void WriteSegment()
        {
            while (true)
            {
                SegmentDownloadInfo segmentDownloadInfo;
                lock (m_SegmentWriterLocker)
                {
                    while (m_OutputBufferQueue.Count == 0)
                    {
                        Monitor.Wait(m_SegmentWriterLocker);
                    }

                    segmentDownloadInfo = m_OutputBufferQueue.Dequeue();
                }

                if (segmentDownloadInfo == null)
                {
                    return;
                }

                lock (m_SegmentWriter)
                {
                    if (segmentDownloadInfo.CurrentPosition != -1)
                    {
                        m_SegmentWriter.Write(segmentDownloadInfo.CurrentPosition, segmentDownloadInfo.Buffer, segmentDownloadInfo.Size);
                        EnqueueDownloadSegment(new byte[m_BufferSize]);
                    }
                }
            }
        }
    }
}
