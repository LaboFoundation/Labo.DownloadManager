namespace Labo.DownloadManager.Segment
{
    using System;

    /// <summary>
    /// Bölüt indirme görevi arayüzü.
    /// </summary>
    public interface ISegmentDownloadTask
    {
        /// <summary>
        /// Bölüt indirme görevini baþlatýr.
        /// </summary>
        void Download();

        /// <summary>
        /// Ýndirmeyi duraklatýr.
        /// </summary>
        void Pause();

        /// <summary>
        /// Ýndirme deneme sayýsý.
        /// </summary>
        int TryCount { get; }

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
        /// Alýnan hatayý atar.
        /// </summary>
        /// <param name="exception">Hata.</param>
        void SetError(Exception exception);

        /// <summary>
        /// Ýndirme deneme sayýsýný arttýrýr.
        /// </summary>
        /// <returns>Arttýrma sonucundaki indirme deneme sayýsý.</returns>
        int IncreaseTryCount();

        /// <summary>
        /// Bölüt indirme bilgisini getirir.
        /// </summary>
        /// <value>
        /// Bölüt indirme bilgisi.
        /// </value>
        ISegmentDownloaderInfo SegmentDownloaderInfo { get; }
    }
}