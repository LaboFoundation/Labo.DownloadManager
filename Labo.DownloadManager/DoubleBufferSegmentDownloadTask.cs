using System.Collections.Generic;
using System.Threading;
namespace Labo.DownloadManager
{
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

        public void Download()
        {
            EnqueueDownloadSegment(new byte[m_BufferSize]);
            EnqueueDownloadSegment(new byte[m_BufferSize]);

            (m_Threads[0] = new Thread(DownloadSegment)).Start();
            (m_Threads[1] = new Thread(WriteSegment)).Start();
        }

        private void DownloadSegment()
        {
            while (true)
            {
                byte[] buffer;
                lock (m_SegmentDownloaderLocker)
                {
                    if (m_InputBufferQueue.Count == 0)
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
                    m_SegmentDownloader.IncreaseCurrentPosition(size);
                    EnqueueWriteSegment(new SegmentDownloadInfo
                        {
                            Buffer = buffer,
                            CurrentPosition = m_SegmentDownloader.CurrentPosition,
                            Size = size
                        });

                    if (m_SegmentDownloader.IsDownloadFinished)
                    {
                        EnqueueWriteSegment(null);
                    }
                }
            }
        }

        private void EnqueueWriteSegment(SegmentDownloadInfo segmentDownloadInfo)
        {
            lock (m_SegmentWriterLocker)
            {
                m_OutputBufferQueue.Enqueue(segmentDownloadInfo);
            }
        }

        private void EnqueueDownloadSegment(byte[] buffer)
        {
            lock (m_SegmentDownloaderLocker)
            {
                m_InputBufferQueue.Enqueue(buffer);
            }
        }

        private void WriteSegment()
        {
            while (true)
            {
                SegmentDownloadInfo segmentDownloadInfo;
                lock (m_SegmentWriterLocker)
                {
                    if (m_OutputBufferQueue.Count == 0)
                    {
                        Monitor.Wait(m_SegmentWriterLocker);
                    }
                    segmentDownloadInfo = m_OutputBufferQueue.Dequeue();
                }
                if (segmentDownloadInfo == null)
                {
                    EnqueueDownloadSegment(null);
                    return;
                }
                lock (m_SegmentWriter)
                {
                    m_SegmentWriter.Write(segmentDownloadInfo.CurrentPosition, segmentDownloadInfo.Buffer, segmentDownloadInfo.Size);
                    EnqueueDownloadSegment(new byte[m_BufferSize]);
                }
                
            }
        }
    }
}
