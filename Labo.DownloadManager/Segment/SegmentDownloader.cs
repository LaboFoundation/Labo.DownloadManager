using System;
using System.IO;
using Labo.DownloadManager.Protocol;

namespace Labo.DownloadManager.Segment
{
    public sealed class SegmentDownloader : SegmentDownloaderBase
    {
        private volatile Stream m_Stream;
        private readonly DownloadFileInfo m_File;
        private readonly INetworkProtocolProvider m_NetworkProtocolProvider;
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

        public SegmentDownloader(DownloadFileInfo file, INetworkProtocolProvider networkProtocolProvider, DownloadSegmentPositions downloadSegmentInfo, ISegmentDownloadRateCalculator segmentDownloadRateCalculator)
            : this(null, downloadSegmentInfo, segmentDownloadRateCalculator)
        {
            m_File = file;
            m_NetworkProtocolProvider = networkProtocolProvider;
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

        private Stream Stream
        {
            get
            {
                if (m_Stream == null)
                {
                    lock (this)
                    {
                        if (m_Stream == null)
                        {
                            m_Stream = m_NetworkProtocolProvider.CreateStream(m_File, m_StartPosition, m_EndPosition);
                        }
                    }
                }
                return m_Stream;
            }
        }

        public override int Download(byte[] buffer)
        {
            if (EndPosition == -1)
            {
                return 0;
            }

            int readLength;
            if (buffer.Length + CurrentPosition - 1 > EndPosition)
            {
                readLength = (int)(EndPosition - CurrentPosition + 1);
            }
            else
            {
                readLength = buffer.Length;
            }

            int readSize = Stream.Read(buffer, 0, readLength);

            if (readSize + CurrentPosition - 1 > EndPosition)
            {
                readSize = (int)(EndPosition - CurrentPosition + 1);
            }

            if (readSize < 0)
            {
                throw new InvalidOperationException("readsize cannot be less than 0");
            }

            return readSize;
        }

        public override void IncreaseCurrentPosition(int size)
        {
            lock (this)
            {
                m_CurrentPosition += size;
            }
        }
    }
}