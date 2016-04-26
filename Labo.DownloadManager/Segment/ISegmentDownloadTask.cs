namespace Labo.DownloadManager.Segment
{
    using System;

    /// <summary>
    /// B�l�t indirme g�revi aray�z�.
    /// </summary>
    public interface ISegmentDownloadTask
    {
        /// <summary>
        /// B�l�t indirme g�revini ba�lat�r.
        /// </summary>
        void Download();

        /// <summary>
        /// �ndirmeyi duraklat�r.
        /// </summary>
        void Pause();

        /// <summary>
        /// �ndirme deneme say�s�.
        /// </summary>
        int TryCount { get; }

        /// <summary>
        /// Al�nan son hatay� getirir.
        /// </summary>
        /// <value>
        /// Al�nan son hata.
        /// </value>
        Exception LastException { get; }

        /// <summary>
        /// Al�nan son hatan�n zaman�n� getirir.
        /// </summary>
        /// <value>
        /// Al�nan son hatan�n zaman�.
        /// </value>
        DateTime? LastExceptionTime { get; }

        /// <summary>
        /// Al�nan hatay� atar.
        /// </summary>
        /// <param name="exception">Hata.</param>
        void SetError(Exception exception);

        /// <summary>
        /// �ndirme deneme say�s�n� artt�r�r.
        /// </summary>
        /// <returns>Artt�rma sonucundaki indirme deneme say�s�.</returns>
        int IncreaseTryCount();

        /// <summary>
        /// B�l�t indirme bilgisini getirir.
        /// </summary>
        /// <value>
        /// B�l�t indirme bilgisi.
        /// </value>
        ISegmentDownloaderInfo SegmentDownloaderInfo { get; }
    }
}