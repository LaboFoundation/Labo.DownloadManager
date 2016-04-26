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

        private readonly int m_MaxRetryCount;

        private readonly int m_RetryDelayTimeInMilliseconds;

        private readonly ISegmentDownloader m_SegmentDownloader;
        private readonly ISegmentWriter m_SegmentWriter;
        private readonly Thread[] m_Threads;

        private readonly Queue<byte[]> m_InputBufferQueue;
        private readonly Queue<SegmentDownloadInfo> m_OutputBufferQueue;
        private readonly object m_SegmentDownloaderLocker = new object();
        private readonly object m_SegmentWriterLocker = new object();

        private bool m_IsRunning;

        private int m_TryCount;

        private Exception m_LastException;

        private DateTime? m_LastExceptionTime;

        /// <summary>
        /// İndirme deneme sayısı.
        /// </summary>
        public int TryCount
        {
            get { return m_TryCount; }
        }

        /// <summary>
        /// Alınan son hatayı getirir.
        /// </summary>
        /// <value>
        /// Alınan son hata.
        /// </value>
        public Exception LastException
        {
            get { return m_LastException; }
        }

        /// <summary>
        /// Alınan son hatanın zamanını getirir.
        /// </summary>
        /// <value>
        /// Alınan son hatanın zamanı.
        /// </value>
        public DateTime? LastExceptionTime
        {
            get { return m_LastExceptionTime; }
        }

        /// <summary>
        /// İndirme deneme sayısını arttırır.
        /// </summary>
        /// <returns>Arttırma sonucundaki indirme deneme sayısı.</returns>
        public int IncreaseTryCount()
        {
            return Interlocked.Increment(ref m_TryCount);
        }

        /// <summary>
        /// Yeni bir <see cref="DoubleBufferSegmentDownloadTask"/> sınıfı örneği yaratır.
        /// </summary>
        /// <param name="bufferSize">Tampon boyutu.</param>
        /// <param name="retryDelayTimeInMilliseconds">Tekrar deneme için geciktirme süresi.</param>
        /// <param name="segmentDownloader">Bölüt indirici.</param>
        /// <param name="segmentWriter">Bölüt yazıcı.</param>
        /// <param name="maxRetryCount">Maksimum tekrar deneme sayısı.</param>
        public DoubleBufferSegmentDownloadTask(int bufferSize, int maxRetryCount, int retryDelayTimeInMilliseconds, ISegmentDownloader segmentDownloader, ISegmentWriter segmentWriter)
        {
            m_BufferSize = bufferSize;
            m_MaxRetryCount = maxRetryCount;
            m_RetryDelayTimeInMilliseconds = retryDelayTimeInMilliseconds;
            m_SegmentDownloader = segmentDownloader;
            m_SegmentWriter = segmentWriter;
            m_InputBufferQueue = new Queue<byte[]>();
            m_OutputBufferQueue = new Queue<SegmentDownloadInfo>();
            m_Threads = new Thread[2];
            m_TryCount = 0;
        }

        /// <summary>
        /// Gets the segment downloader information.
        /// </summary>
        /// <value>
        /// The segment downloader information.
        /// </value>
        public ISegmentDownloaderInfo SegmentDownloaderInfo
        {
            get
            {
                return m_SegmentDownloader;
            }
        }

        /// <summary>
        /// Starts the segment download task.
        /// </summary>
        public void Download()
        {
            EnqueueDownloadSegment(new byte[m_BufferSize]);
            EnqueueDownloadSegment(new byte[m_BufferSize]);

            m_IsRunning = true;

            m_SegmentDownloader.State = SegmentState.Downloading;

            (m_Threads[0] = new Thread(DownloadSegment)).Start();
            (m_Threads[1] = new Thread(WriteSegment)).Start();

            Shutdown(true);
        }

        public void Pause()
        {
            m_IsRunning = false;
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

        /// <summary>
        /// Alınan hatayı atar.
        /// </summary>
        /// <param name="exception">Hata.</param>
        public void SetError(Exception exception)
        {
            m_LastException = exception;
            m_LastExceptionTime = DateTime.Now;
        }

        private void DownloadSegment()
        {
            while (true)
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

                if (!m_IsRunning)
                {
                    return;
                }

                bool retry = false;

            retry:
                try
                {
                    //lock (m_SegmentDownloader)
                    //{

                    if (retry)
                    {
                        Thread.Sleep(m_RetryDelayTimeInMilliseconds);

                        m_SegmentDownloader.RefreshDownloadStream();
                    }

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
                    //}
                }
                catch (Exception ex)
                {
                    SetError(ex);

                    m_SegmentDownloader.SetError(ex);

                    if (!m_SegmentDownloader.IsDownloadFinished && IncreaseTryCount() < m_MaxRetryCount)
                    {
                        retry = true;
                        goto retry;
                    }
                   
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
                    m_SegmentDownloader.SetDownloadFinishDate(DateTime.Now);                        

                    return;
                }

                lock (m_SegmentWriter)
                {
                    if (segmentDownloadInfo.CurrentPosition != -1)
                    {
                        m_SegmentWriter.Write(segmentDownloadInfo.CurrentPosition, segmentDownloadInfo.Buffer, segmentDownloadInfo.Size);
                        m_SegmentDownloader.CalculateCurrentDownloadRate(segmentDownloadInfo.CurrentPosition);

                        if (m_IsRunning)
                        {
                            EnqueueDownloadSegment(new byte[m_BufferSize]);                            
                        }
                    }
                }

                if (!m_IsRunning)
                {
                    m_SegmentDownloader.SetDownloadFinishDate(DateTime.Now);

                    return;
                }
            }
        }
    }
}
