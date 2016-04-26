namespace Labo.DownloadManager.Segment
{
    using System;

    /// <summary>
    /// Bölüt indirme bilgisi arayüzü.
    /// </summary>
    public interface ISegmentDownloaderInfo
    {
        /// <summary>
        /// Bölüt indiricisinin mevcut konumunu getirir.
        /// </summary>
        long CurrentPosition { get; }

        /// <summary>
        /// Bölütün boyutu.
        /// </summary>
        long Size { get; }

        /// <summary>
        /// Bölütü indirmek için kalan byte sayýsýný getirir.
        /// </summary>
        long RemainingTransfer { get; }

        /// <summary>
        /// Bölütün indirilen byte sayýsýný getirir.
        /// </summary>
        long TransferedDownload { get; }

        /// <summary>
        /// Bölütün kalan tahmini indirme süresini getirir. 
        /// Kalan byte ve ortalama indirme hýzýna göre bir hesaplama yapar.
        /// </summary>
        TimeSpan? RemainingTime { get; }

        /// <summary>
        /// [Ýndirmenin bitiþine] dair bir deðer döner.
        /// </summary>
        /// <value>
        /// Eðer [indirme bitti] ise <c>true</c> ; yoksa, <c>false</c>.
        /// </value>
        bool IsDownloadFinished { get; }

        /// <summary>
        /// Ýndirme hýzýný getirir.
        /// </summary>
        /// <value>
        /// Ýndirme hýzý.
        /// </value>
        double? DownloadRate { get; }

        /// <summary>
        /// Ýndirme yüzdesini getirir.
        /// </summary>
        /// <value>
        /// Ýndirme yüzdesi.
        /// </value>
        double DownloadProgress { get; }

        /// <summary>
        /// Bölüt indirici için baþlangýç konumu.
        /// </summary>
        /// <value>
        /// Baþlangýç konumu.
        /// </value>
        long StartPosition { get; }

        /// <summary>
        /// Bölüt indirici için bitiþ konumu.
        /// </summary>
        /// <value>
        /// Bitiþ konumu.
        /// </value>
        long EndPosition { get; }

        /// <summary>
        /// Bölüt indiricinin durumunu getirir.
        /// </summary>
        /// <value>
        /// Bölüt indirici durumu.
        /// </value>
        SegmentState State { get; set; }

        /// <summary>
        /// Alýnan son hatayý getirir.
        /// </summary>
        /// <value>
        /// Alýnan son hata.
        /// </value>
        Exception LastException { get; }

        /// <summary>
        /// Alýnan son hatanýn zamanýný getirir.
        /// </summary>
        /// <value>
        /// Alýnan son hatanýn zamaný.
        /// </value>
        DateTime? LastExceptionTime { get; }

        /// <summary>
        /// Bölütün indirme yaptýðý adresini getirir.
        /// </summary>
        /// <value>
        /// Ýndirme adresi.
        /// </value>
        Uri Uri { get; }

        /// <summary>
        /// Ýndirmenin bitiþ zamaný.
        /// </summary>
        /// <value>
        /// Ýndirme bitiþ zamaný.
        /// </value>
        DateTime? DownloadFinishDate { get; }
    }
}