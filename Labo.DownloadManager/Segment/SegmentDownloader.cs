namespace Labo.DownloadManager.Segment
{
    using System;
    using System.IO;

    using Labo.DownloadManager.Protocol;

    /// <summary>
    /// Bölüt indirici sınıfı.
    /// </summary>
    public sealed class SegmentDownloader : SegmentDownloaderBase
    {
        /// <summary>
        /// Dosya indirme bilgisi.
        /// </summary>
        private readonly DownloadFileInfo m_File;

        /// <summary>
        /// Ağ protokolü sağlayıcısı.
        /// </summary>
        private readonly INetworkProtocolProvider m_NetworkProtocolProvider;

        /// <summary>
        /// Bölüt indirme hızı hesaplayıcısı.
        /// </summary>
        private readonly ISegmentDownloadRateCalculator m_SegmentDownloadRateCalculator;

        /// <summary>
        /// Bölüt başlangıç konumu.
        /// </summary>
        private readonly long m_StartPosition;

        /// <summary>
        /// Bölüt bitiş konumu.
        /// </summary>
        private readonly long m_EndPosition;

        /// <summary>
        /// Mevcut indirme konumu.
        /// </summary>
        private long m_CurrentPosition;

        /// <summary>
        /// TBölüt indirme durumu.
        /// </summary>
        private SegmentState m_SegmentState;

        /// <summary>
        /// Alınan son hata.
        /// </summary>
        private Exception m_LastException;

        /// <summary>
        /// Son hata alma zamanı.
        /// </summary>
        private DateTime? m_LastExceptionTime;

        /// <summary>
        /// İndirme kaynağı.
        /// </summary>
        private volatile Stream m_Stream;

        /// <summary>
        /// Eşzamanlama kilit objesi.
        /// </summary>
        private readonly object m_SyncLock = new object();

        private double? m_Rate;

        /// <summary>
        /// Yeni bir <see cref="SegmentDownloader"/> sınıfı yaratır.
        /// </summary>
        /// <param name="stream">İndirme kaynağı.</param>
        /// <param name="downloadSegmentInfo">Bölüt indirme bilgisi.</param>
        /// <param name="segmentDownloadRateCalculator">Bölüt indirme hızı hesaplayıcı.</param>
        /// <exception cref="System.ArgumentNullException">downloadSegmentInfo</exception>
        internal SegmentDownloader(Stream stream, DownloadSegmentPositions downloadSegmentInfo, ISegmentDownloadRateCalculator segmentDownloadRateCalculator)
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
        /// Yeni bir <see cref="SegmentDownloader"/> sınıfı yaratır.
        /// </summary>
        /// <param name="file">Dosya indirme bilğisi.</param>
        /// <param name="networkProtocolProvider">Ağ protokolü sağlayıcısı.</param>
        /// <param name="downloadSegmentInfo">Bölüt indirme bilgisi.</param>
        /// <param name="segmentDownloadRateCalculator">Bölüt indirme hızı hesaplayıcı.</param>
        public SegmentDownloader(DownloadFileInfo file, INetworkProtocolProvider networkProtocolProvider, DownloadSegmentPositions downloadSegmentInfo, ISegmentDownloadRateCalculator segmentDownloadRateCalculator)
            : this(null, downloadSegmentInfo, segmentDownloadRateCalculator)
        {
            m_File = file;
            m_NetworkProtocolProvider = networkProtocolProvider;
        }

        /// <summary>
        /// Bölüt indiricisinin mevcut konumunu getirir.
        /// </summary>
        public override long CurrentPosition
        {
            get { return m_CurrentPosition; }
        }

        /// <summary>
        /// İndirme hızını getirir.
        /// </summary>
        /// <value>
        /// İndirme hızı.
        /// </value>
        public override double? DownloadRate
        {
            get
            {
                return m_Rate;
            }

            protected set
            {
                m_Rate = value;
            }
        }

        /// <summary>
        /// Bölüt indirici için başlangıç konumu.
        /// </summary>
        /// <value>
        /// Başlangıç konumu.
        /// </value>
        public override long StartPosition
        {
            get { return m_StartPosition; }
        }

        /// <summary>
        /// Bölüt indirici için bitiş konumu.
        /// </summary>
        /// <value>
        /// Bitiş konumu.
        /// </value>
        public override long EndPosition
        {
            get { return m_EndPosition; }
        }

        /// <summary>
        /// Mevcut kaynaktan istenen sayıda veriyi okuyup tampon dizisine
        /// yazdıktan sonra kaynağın konumunu okunan byte sayısı kadar arttırır.
        /// </summary>
        /// <param name="buffer">
        /// Mevcut kaynaktan okunup içerisine aktarılacak olan tampon dizisi.
        /// </param>
        /// <returns>
        /// Kaynaktan okunup tampon dizisinin için aktarılan byte sayısı. 
        /// Bu değer eğer kaynakta yeterli sayıda veri yoksa talep edilen byte sayısından az dönebilir.
        /// Eğer kaynağın sonuna gelindi ise (0) döner.
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

            // CalculateCurrentDownloadRate();

            return readSize;
        }

        public override void CalculateCurrentDownloadRate(long currentPosition)
        {
            m_Rate = m_SegmentDownloadRateCalculator.CalculateDownloadRate(currentPosition);
        }

        /// <summary>
        /// Bölüt indiricinin mevcut konumunu verilen değer kadar arttırır.
        /// </summary>
        /// <param name="size">Artış değeri.</param>
        public override void IncreaseCurrentPosition(int size)
        {
            //lock (this)
            //{
                m_CurrentPosition += size;
            //}
        }

        /// <summary>
        /// Bölüt indiricinin durumunu getirir.
        /// </summary>
        /// <value>
        /// Bölüt indirici durumu.
        /// </value>
        public override SegmentState State
        {
            get
            {
                return m_SegmentState;
            }
            set
            {
                m_SegmentState = value;
            }
        }

        /// <summary>
        /// Alınan son hatayı getirir.
        /// </summary>
        /// <value>
        /// Alınan son hata.
        /// </value>
        public override Exception LastException
        {
            get { return m_LastException; }
        }

        /// <summary>
        /// Alınan son hatanın zamanını getirir.
        /// </summary>
        /// <value>
        /// Alınan son hatanın zamanı.
        /// </value>
        public override DateTime? LastExceptionTime
        {
            get { return m_LastExceptionTime; }
        }

        /// <summary>
        /// Bölütün indirme yaptığı adresini getirir.
        /// </summary>
        /// <value>
        /// İndirme adresi.
        /// </value>
        public override Uri Uri
        {
            get { return m_File == null ? null : m_File.Uri; }
        }

        /// <summary>
        /// Alınan hatayı atar.
        /// </summary>
        /// <param name="exception">Hata.</param>
        public override void SetError(Exception exception)
        {
            m_LastException = exception;
            m_LastExceptionTime = DateTime.Now;
            m_SegmentState = SegmentState.Error;
        }

        /// <summary>
        /// İndirme kaynağını günceller.
        /// </summary>
        public override void RefreshDownloadStream()
        {
            lock (m_SyncLock)
            {
                m_Stream = CreateStream();
            }
        }

        /// <summary>
        /// Bölüt indirme kaynağı.
        /// </summary>
        /// <value>
        /// İndirme kaynağı.
        /// </value>
        private Stream Stream
        {
            get
            {
                if (m_Stream == null)
                {
                    lock (m_SyncLock)
                    {
                        if (m_Stream == null)
                        {
                            m_Stream = CreateStream();
                        }
                    }
                }

                return m_Stream;
            }
        }

        private Stream CreateStream()
        {
            return m_NetworkProtocolProvider.CreateStream(
                new DownloadFileRequestInfo(m_File),
                m_CurrentPosition, 
                m_EndPosition);
        }
    }
}