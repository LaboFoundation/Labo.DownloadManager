namespace Labo.DownloadManager.Segment
{
    using System;
    using System.IO;

    using Labo.DownloadManager.Protocol;

    /// <summary>
    /// The segment downloader class.
    /// </summary>
    public sealed class SegmentDownloader : SegmentDownloaderBase
    {
        /// <summary>
        /// The download file info.
        /// </summary>
        private readonly DownloadFileInfo m_File;

        /// <summary>
        /// The network protocol provider
        /// </summary>
        private readonly INetworkProtocolProvider m_NetworkProtocolProvider;

        /// <summary>
        /// The segment download rate calculator
        /// </summary>
        private readonly ISegmentDownloadRateCalculator m_SegmentDownloadRateCalculator;

        /// <summary>
        /// The segment start position
        /// </summary>
        private readonly long m_StartPosition;

        /// <summary>
        /// The segment end position
        /// </summary>
        private readonly long m_EndPosition;

        /// <summary>
        /// The current download position
        /// </summary>
        private long m_CurrentPosition;

        /// <summary>
        /// The segment download state
        /// </summary>
        private SegmentState m_SegmentState;

        /// <summary>
        /// The last exception
        /// </summary>
        private Exception m_LastException;

        /// <summary>
        /// The last exception time
        /// </summary>
        private DateTime? m_LastExceptionTime;

        /// <summary>
        /// The download stream
        /// </summary>
        private volatile Stream m_Stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentDownloader"/> class.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="downloadSegmentInfo">The download segment information.</param>
        /// <param name="segmentDownloadRateCalculator">The segment download rate calculator.</param>
        /// <exception cref="System.ArgumentNullException">downloadSegmentInfo</exception>
        public SegmentDownloader(Stream stream, DownloadSegmentPositions downloadSegmentInfo, ISegmentDownloadRateCalculator segmentDownloadRateCalculator)
        {
            if (downloadSegmentInfo == null)
            {
                throw new ArgumentNullException("downloadSegmentInfo");
            }

            m_Stream = stream;
            m_SegmentDownloadRateCalculator = segmentDownloadRateCalculator;
            m_StartPosition = downloadSegmentInfo.StartPosition;
            m_EndPosition = downloadSegmentInfo.EndPosition;
            m_CurrentPosition = StartPosition;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SegmentDownloader"/> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="networkProtocolProvider">The network protocol provider.</param>
        /// <param name="downloadSegmentInfo">The download segment information.</param>
        /// <param name="segmentDownloadRateCalculator">The segment download rate calculator.</param>
        public SegmentDownloader(DownloadFileInfo file, INetworkProtocolProvider networkProtocolProvider, DownloadSegmentPositions downloadSegmentInfo, ISegmentDownloadRateCalculator segmentDownloadRateCalculator)
            : this(null, downloadSegmentInfo, segmentDownloadRateCalculator)
        {
            m_File = file;
            m_NetworkProtocolProvider = networkProtocolProvider;
        }

        /// <summary>
        /// Gets the current position of the segment downloader.
        /// </summary>
        public override long CurrentPosition
        {
            get { return m_CurrentPosition; }
        }

        /// <summary>
        /// Gets the download rate.
        /// </summary>
        /// <value>
        /// The download rate.
        /// </value>
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

        /// <summary>
        /// Gets the start position of the download segment.
        /// </summary>
        /// <value>
        /// The start position.
        /// </value>
        public override long StartPosition
        {
            get { return m_StartPosition; }
        }

        /// <summary>
        /// Gets the end position of the download segment.
        /// </summary>
        /// <value>
        /// The end position.
        /// </value>
        public override long EndPosition
        {
            get { return m_EndPosition; }
        }

        /// <summary>
        /// Reads a sequence of bytes from the current stream and advances 
        /// the position within the stream by the number of bytes read.
        /// </summary>
        /// <param name="buffer">
        /// An array of bytes that contains the specified byte array with the values which will be replaced 
        /// by the bytes read from the current source when the method returns.
        /// </param>
        /// <returns>
        /// The total number of bytes read into the buffer. This can be less than the number of bytes requested if 
        /// that many bytes are not currently available, or zero (0) if the end of the stream has been reached.
        /// </returns>
        public override int Download(byte[] buffer)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("buffer");
            }

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

        /// <summary>
        /// Increases the current position of the segment downloader.
        /// </summary>
        /// <param name="size">The increase size.</param>
        public override void IncreaseCurrentPosition(int size)
        {
            lock (this)
            {
                m_CurrentPosition += size;
            }
        }

        /// <summary>
        /// Gets the state of the download segment.
        /// </summary>
        /// <value>
        /// The download segment state.
        /// </value>
        public override SegmentState State
        {
            get { return m_SegmentState; }
        }

        /// <summary>
        /// Gets the last exception.
        /// </summary>
        /// <value>
        /// The last exception.
        /// </value>
        public override Exception LastException
        {
            get { return m_LastException; }
        }

        /// <summary>
        /// Gets the last exception time.
        /// </summary>
        /// <value>
        /// The last exception time.
        /// </value>
        public override DateTime? LastExceptionTime
        {
            get { return m_LastExceptionTime; }
        }

        /// <summary>
        /// Gets the download URI of the segment.
        /// </summary>
        /// <value>
        /// The URL.
        /// </value>
        public override Uri Uri
        {
            get { return m_File == null ? null : m_File.Uri; }
        }

        /// <summary>
        /// Gets the segment download stream.
        /// </summary>
        /// <value>
        /// The stream.
        /// </value>
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
                            m_Stream = m_NetworkProtocolProvider.CreateStream(m_File, m_CurrentPosition, m_EndPosition);
                        }
                    }
                }

                return m_Stream;
            }
        }

        /// <summary>
        /// Sets the error.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public override void SetError(Exception exception)
        {
            m_LastException = exception;
            m_LastExceptionTime = DateTime.Now;
            m_SegmentState = SegmentState.Error;
        }
    }
}