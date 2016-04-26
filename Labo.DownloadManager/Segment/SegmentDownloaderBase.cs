namespace Labo.DownloadManager.Segment
{
    using System;

    /// <summary>
    /// Bölüt indirici üs sýnýfý.
    /// </summary>
    public abstract class SegmentDownloaderBase : ISegmentDownloader
    {
        /// <summary>
        /// Bölüt indiricisinin mevcut konumunu getirir.
        /// </summary>
        public abstract long CurrentPosition { get; }

        /// <summary>
        /// Bölütü indirmek için kalan byte sayýsýný getirir.
        /// </summary>
        public long RemainingTransfer
        {
            get
            {
                return EndPosition <= 0 ? 0 : EndPosition - CurrentPosition;
            }
        }

        /// <summary>
        /// [Ýndirmenin bitiþine] dair bir deðer döner.
        /// </summary>
        /// <value>
        /// Eðer [indirme bitti] ise <c>true</c>; yoksa, <c>false</c>.
        /// </value>
        public bool IsDownloadFinished
        {
            get { return EndPosition == -1 || CurrentPosition > EndPosition; }
        }

        /// <summary>
        /// Ýndirme hýzýný getirir.
        /// </summary>
        /// <value>
        /// Ýndirme hýzý.
        /// </value>
        public abstract double? DownloadRate { get; protected set; }

        /// <summary>
        /// Bölütün kalan tahmini indirme süresini getirir. 
        /// Kalan byte sayýsý ve ortalama indirme hýzýna göre bir hesaplama yapar.
        /// </summary>
        public TimeSpan? RemainingTime
        {
            get
            {
                double? dowloadRate = DownloadRate;
                if (!dowloadRate.HasValue)
                {
                    return null;
                }

                if (dowloadRate.Value > 0.0)
                {
                    return TimeSpan.FromSeconds(RemainingTransfer / dowloadRate.Value);
                }

                return TimeSpan.MaxValue;
            }
        }

        /// <summary>
        /// Bölütün indirilen byte sayýsýný getirir.
        /// </summary>
        public long TransferedDownload
        {
            get { return CurrentPosition - StartPosition; }
        }

        /// <summary>
        /// Ýndirme yüzdesini getirir.
        /// </summary>
        /// <value>
        /// Ýndirme yüzdesi.
        /// </value>
        public double DownloadProgress
        {
            get
            {
                return EndPosition <= 0 ? 0 : (TransferedDownload / (double)Size * 100.0);
            }
        }

        /// <summary>
        /// Bölüt indirici için baþlangýç konumu.
        /// </summary>
        /// <value>
        /// Baþlangýç konumu.
        /// </value>
        public abstract long StartPosition { get; }

        /// <summary>
        /// Bölüt indirici için bitiþ konumu.
        /// </summary>
        /// <value>
        /// Bitiþ konumu.
        /// </value>
        public abstract long EndPosition { get; }

        /// <summary>
        /// Mevcut kaynaktan istenen sayýda veriyi okuyup tampon dizisine
        /// yazdýktan sonra kaynaðýn konumunu okunan byte sayýsý kadar arttýrýr.
        /// </summary>
        /// <param name="buffer">
        /// Mevcut kaynaktan okunup içerisine aktarýlacak olan tampon dizisi.
        /// </param>
        /// <returns>
        /// Kaynaktan okunup tampon dizisinin için aktarýlan byte sayýsý. 
        /// Bu deðer eðer kaynakta yeterli sayýda veri yoksa talep edilen byte sayýsýndan az dönebilir.
        /// Eðer kaynaðýn sonuna gelindi ise (0) döner.
        /// </returns>
        public abstract int Download(byte[] buffer);

        /// <summary>
        /// Bölüt indiricinin mevcut konumunu verilen deðer kadar arttýrýr.
        /// </summary>
        /// <param name="size">Artýþ deðeri.</param>
        public abstract void IncreaseCurrentPosition(int size);

        /// <summary>
        /// Alýnan hatayý atar.
        /// </summary>
        /// <param name="exception">Hata.</param>
        public abstract void SetError(Exception exception);

        /// <summary>
        /// Bölüt indiricinin durumunu getirir.
        /// </summary>
        /// <value>
        /// Bölüt indirici durumu.
        /// </value>
        public abstract SegmentState State { get; set; }

        /// <summary>
        /// Alýnan son hatayý getirir.
        /// </summary>
        /// <value>
        /// Alýnan son hata.
        /// </value>
        public abstract Exception LastException { get; }

        /// <summary>
        /// Alýnan son hatanýn zamanýný getirir.
        /// </summary>
        /// <value>
        /// Alýnan son hatanýn zamaný.
        /// </value>
        public abstract DateTime? LastExceptionTime { get; }

        /// <summary>
        /// Bölütün indirme yaptýðý adresini getirir.
        /// </summary>
        /// <value>
        /// Ýndirme adresi.
        /// </value>
        public abstract Uri Uri { get; }

        /// <summary>
        /// Ýndirme kaynaðýný günceller.
        /// </summary>
        public abstract void RefreshDownloadStream();

        /// <summary>
        /// Ýndirmenin bitiþ zamaný.
        /// </summary>
        /// <value>
        /// Ýndirme bitiþ zamaný.
        /// </value>
        public DateTime? DownloadFinishDate { get; private set; }

        /// <summary>
        /// Ýndirme bitiþ zamanýný atar.
        /// </summary>
        /// <param name="date">Bitiþ zamaný.</param>
        public void SetDownloadFinishDate(DateTime date)
        {
            DownloadFinishDate = date;
            State = SegmentState.Finished;

            DownloadRate = null;
        }

        /// <summary>
        /// Bölütün boyutu.
        /// </summary>
        public long Size
        {
            get { return EndPosition - StartPosition; }
        }

        public abstract void CalculateCurrentDownloadRate(long currentPosition);
    }
}