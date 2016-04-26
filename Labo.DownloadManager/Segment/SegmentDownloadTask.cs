namespace Labo.DownloadManager.Segment
{
    using System;

    /// <summary>
    /// Bölüt indirme görevi.
    /// </summary>
    public sealed class SegmentDownloadTask : ISegmentDownloadTask
    {
        /// <summary>
        /// Bölüt indirici.
        /// </summary>
        private readonly ISegmentDownloader m_SegmentDownloader;

        /// <summary>
        /// Bölüt yazıcı.
        /// </summary>
        private readonly ISegmentWriter m_SegmentWriter;

        /// <summary>
        /// Tampon boyutu.
        /// </summary>
        private readonly int m_BufferSize;

        /// <summary>
        /// Bölüt indirme görevi devam ediyor mu?
        /// </summary>
        private volatile bool m_IsRunning;

        /// <summary>
        /// Bölüt indirme bilgisini getirir.
        /// </summary>
        /// <value>
        /// Bölüt indirme bilgisi.
        /// </value>
        public ISegmentDownloaderInfo SegmentDownloaderInfo
        {
            get { return m_SegmentDownloader; }
        }

        /// <summary>
        /// Yeni bir <see cref="SegmentDownloadTask"/> sınıfı örneği yaratır.
        /// </summary>
        /// <param name="bufferSize">Tampon boyutu.</param>
        /// <param name="segmentDownloader">Bölüt indirici.</param>
        /// <param name="segmentWriter">Bölüt yazıcı.</param>
        public SegmentDownloadTask(int bufferSize, ISegmentDownloader segmentDownloader, ISegmentWriter segmentWriter)
        {
            if (segmentDownloader == null)
            {
                throw new ArgumentNullException("segmentDownloader");
            }

            if (segmentWriter == null)
            {
                throw new ArgumentNullException("segmentWriter");
            }

            m_BufferSize = bufferSize;
            m_SegmentDownloader = segmentDownloader;
            m_SegmentWriter = segmentWriter;
        }

        /// <summary>
        /// Bölüt indirme görevini başlatır.
        /// </summary>
        public void Download()
        {
            int readSize;
            byte[] buffer = new byte[m_BufferSize];

            m_IsRunning = true;

            do
            {
                readSize = m_SegmentDownloader.Download(buffer);

                //lock (m_SegmentWriter)
                //{
                m_SegmentWriter.Write(m_SegmentDownloader.CurrentPosition, buffer, readSize);
                m_SegmentDownloader.IncreaseCurrentPosition(readSize);

                if (m_SegmentDownloader.IsDownloadFinished)
                {
                    break;
                }
                //}
            }
            while (readSize > 0 && m_IsRunning);

            m_SegmentDownloader.SetDownloadFinishDate(DateTime.Now);
        }

        /// <summary>
        /// İndirmeyi duraklatır.
        /// </summary>
        public void Pause()
        {
            m_IsRunning = false;
        }


        public int TryCount
        {
            get { throw new NotImplementedException(); }
        }

        public Exception LastException
        {
            get { throw new NotImplementedException(); }
        }

        public DateTime? LastExceptionTime
        {
            get { throw new NotImplementedException(); }
        }

        public void SetError(Exception exception)
        {
            throw new NotImplementedException();
        }

        public int IncreaseTryCount()
        {
            throw new NotImplementedException();
        }
    }
}
