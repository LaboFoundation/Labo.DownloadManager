using System;
using System.IO;

namespace Labo.DownloadManager
{
    public sealed class SegmentDownloader : SegmentDownloaderBase
    {
        private readonly Stream m_Stream;
        private readonly ISegmentDownloadRateCalculator m_SegmentDownloadRateCalculator;

        private readonly long m_StartPosition;
        private long m_CurrentPosition;
        private readonly long m_EndPosition;

        public SegmentDownloader(Stream stream, DownloadSegmentPositions downloadSegmentInfo, ISegmentDownloadRateCalculator segmentDownloadRateCalculator)
        {
            m_Stream = stream;
            m_SegmentDownloadRateCalculator = segmentDownloadRateCalculator;
            m_StartPosition = downloadSegmentInfo.StartPosition;
            m_EndPosition = downloadSegmentInfo.EndPosition;
            m_CurrentPosition = StartPosition;
        }

        public override long CurrentPosition
        {
            get { return m_CurrentPosition; }
        }

        public override double? DownloadRate
        {
            get
            {
                lock (this)
                {
                    return m_SegmentDownloadRateCalculator.CalculateDownloadRate(CurrentPosition);
                }
            }
        }

        public override long StartPosition
        {
            get { return m_StartPosition; }
        }

        public override long EndPosition
        {
            get { return m_EndPosition; }
        }

        public override int Download(byte[] buffer)
        {
            int readLength;
            if (buffer.Length + CurrentPosition > EndPosition)
            {
                readLength = (int)(EndPosition - CurrentPosition);
            }
            else
            {
                readLength = buffer.Length;
            }

            int readSize = m_Stream.Read(buffer, 0, readLength);

            if (readSize + CurrentPosition > EndPosition)
            {
                readSize = (int)(EndPosition - CurrentPosition);
            }

            if (readSize < 0)
            {
                throw new InvalidOperationException("readsize cannot be less than 0");
            }

            return readSize;
        }

        public override void IncreaseCurrentPosition(int size)
        {
            m_CurrentPosition += size;
        }
    }
}