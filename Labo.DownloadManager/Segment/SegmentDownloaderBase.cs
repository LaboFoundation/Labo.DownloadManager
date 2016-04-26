namespace Labo.DownloadManager.Segment
{
    using System;

    /// <summary>
    /// B�l�t indirici �s s�n�f�.
    /// </summary>
    public abstract class SegmentDownloaderBase : ISegmentDownloader
    {
        /// <summary>
        /// B�l�t indiricisinin mevcut konumunu getirir.
        /// </summary>
        public abstract long CurrentPosition { get; }

        /// <summary>
        /// B�l�t� indirmek i�in kalan byte say�s�n� getirir.
        /// </summary>
        public long RemainingTransfer
        {
            get
            {
                return EndPosition <= 0 ? 0 : EndPosition - CurrentPosition;
            }
        }

        /// <summary>
        /// [�ndirmenin biti�ine] dair bir de�er d�ner.
        /// </summary>
        /// <value>
        /// E�er [indirme bitti] ise <c>true</c>; yoksa, <c>false</c>.
        /// </value>
        public bool IsDownloadFinished
        {
            get { return EndPosition == -1 || CurrentPosition > EndPosition; }
        }

        /// <summary>
        /// �ndirme h�z�n� getirir.
        /// </summary>
        /// <value>
        /// �ndirme h�z�.
        /// </value>
        public abstract double? DownloadRate { get; protected set; }

        /// <summary>
        /// B�l�t�n kalan tahmini indirme s�resini getirir. 
        /// Kalan byte say�s� ve ortalama indirme h�z�na g�re bir hesaplama yapar.
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
        /// B�l�t�n indirilen byte say�s�n� getirir.
        /// </summary>
        public long TransferedDownload
        {
            get { return CurrentPosition - StartPosition; }
        }

        /// <summary>
        /// �ndirme y�zdesini getirir.
        /// </summary>
        /// <value>
        /// �ndirme y�zdesi.
        /// </value>
        public double DownloadProgress
        {
            get
            {
                return EndPosition <= 0 ? 0 : (TransferedDownload / (double)Size * 100.0);
            }
        }

        /// <summary>
        /// B�l�t indirici i�in ba�lang�� konumu.
        /// </summary>
        /// <value>
        /// Ba�lang�� konumu.
        /// </value>
        public abstract long StartPosition { get; }

        /// <summary>
        /// B�l�t indirici i�in biti� konumu.
        /// </summary>
        /// <value>
        /// Biti� konumu.
        /// </value>
        public abstract long EndPosition { get; }

        /// <summary>
        /// Mevcut kaynaktan istenen say�da veriyi okuyup tampon dizisine
        /// yazd�ktan sonra kayna��n konumunu okunan byte say�s� kadar artt�r�r.
        /// </summary>
        /// <param name="buffer">
        /// Mevcut kaynaktan okunup i�erisine aktar�lacak olan tampon dizisi.
        /// </param>
        /// <returns>
        /// Kaynaktan okunup tampon dizisinin i�in aktar�lan byte say�s�. 
        /// Bu de�er e�er kaynakta yeterli say�da veri yoksa talep edilen byte say�s�ndan az d�nebilir.
        /// E�er kayna��n sonuna gelindi ise (0) d�ner.
        /// </returns>
        public abstract int Download(byte[] buffer);

        /// <summary>
        /// B�l�t indiricinin mevcut konumunu verilen de�er kadar artt�r�r.
        /// </summary>
        /// <param name="size">Art�� de�eri.</param>
        public abstract void IncreaseCurrentPosition(int size);

        /// <summary>
        /// Al�nan hatay� atar.
        /// </summary>
        /// <param name="exception">Hata.</param>
        public abstract void SetError(Exception exception);

        /// <summary>
        /// B�l�t indiricinin durumunu getirir.
        /// </summary>
        /// <value>
        /// B�l�t indirici durumu.
        /// </value>
        public abstract SegmentState State { get; set; }

        /// <summary>
        /// Al�nan son hatay� getirir.
        /// </summary>
        /// <value>
        /// Al�nan son hata.
        /// </value>
        public abstract Exception LastException { get; }

        /// <summary>
        /// Al�nan son hatan�n zaman�n� getirir.
        /// </summary>
        /// <value>
        /// Al�nan son hatan�n zaman�.
        /// </value>
        public abstract DateTime? LastExceptionTime { get; }

        /// <summary>
        /// B�l�t�n indirme yapt��� adresini getirir.
        /// </summary>
        /// <value>
        /// �ndirme adresi.
        /// </value>
        public abstract Uri Uri { get; }

        /// <summary>
        /// �ndirme kayna��n� g�nceller.
        /// </summary>
        public abstract void RefreshDownloadStream();

        /// <summary>
        /// �ndirmenin biti� zaman�.
        /// </summary>
        /// <value>
        /// �ndirme biti� zaman�.
        /// </value>
        public DateTime? DownloadFinishDate { get; private set; }

        /// <summary>
        /// �ndirme biti� zaman�n� atar.
        /// </summary>
        /// <param name="date">Biti� zaman�.</param>
        public void SetDownloadFinishDate(DateTime date)
        {
            DownloadFinishDate = date;
            State = SegmentState.Finished;

            DownloadRate = null;
        }

        /// <summary>
        /// B�l�t�n boyutu.
        /// </summary>
        public long Size
        {
            get { return EndPosition - StartPosition; }
        }

        public abstract void CalculateCurrentDownloadRate(long currentPosition);
    }
}