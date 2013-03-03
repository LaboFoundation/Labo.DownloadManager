using System;
using System.IO;

namespace Labo.DownloadManager
{
    public sealed class SegmentDownloader : ISegmentDownloader
    {
        private readonly Stream m_Stream;

        private readonly long m_StartPosition;
        private long m_CurrentPosition;
        private readonly long m_EndPosition;

        public SegmentDownloader(Stream stream, DownloadSegmentPositions downloadSegmentInfo)
        {
            m_Stream = stream;
            m_StartPosition = downloadSegmentInfo.StartPosition;
            m_EndPosition = downloadSegmentInfo.EndPosition;
            m_CurrentPosition = m_StartPosition;
        }

        public long CurrentPosition
        {
            get { return m_CurrentPosition; }
        }

        public bool IsDownloadFinished
        {
            get { return m_CurrentPosition >= m_EndPosition; }
        }

        public int Download(byte[] buffer)
        {
            int readSize = m_Stream.Read(buffer, 0, buffer.Length);

            if (readSize + CurrentPosition > m_EndPosition)
            {
                readSize = (int)(m_EndPosition - CurrentPosition);
            }

            if (readSize < 0)
            {
                throw new InvalidOperationException("readsize cannot be less than 0");
            }

            return readSize;
        }

        public void IncreaseCurrentPosition(int size)
        {
            m_CurrentPosition += size;
        }
    }
}
