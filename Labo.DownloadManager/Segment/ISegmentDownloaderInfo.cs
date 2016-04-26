namespace Labo.DownloadManager.Segment
{
    using System;

    /// <summary>
    /// B�l�t indirme bilgisi aray�z�.
    /// </summary>
    public interface ISegmentDownloaderInfo
    {
        /// <summary>
        /// B�l�t indiricisinin mevcut konumunu getirir.
        /// </summary>
        long CurrentPosition { get; }

        /// <summary>
        /// B�l�t�n boyutu.
        /// </summary>
        long Size { get; }

        /// <summary>
        /// B�l�t� indirmek i�in kalan byte say�s�n� getirir.
        /// </summary>
        long RemainingTransfer { get; }

        /// <summary>
        /// B�l�t�n indirilen byte say�s�n� getirir.
        /// </summary>
        long TransferedDownload { get; }

        /// <summary>
        /// B�l�t�n kalan tahmini indirme s�resini getirir. 
        /// Kalan byte ve ortalama indirme h�z�na g�re bir hesaplama yapar.
        /// </summary>
        TimeSpan? RemainingTime { get; }

        /// <summary>
        /// [�ndirmenin biti�ine] dair bir de�er d�ner.
        /// </summary>
        /// <value>
        /// E�er [indirme bitti] ise <c>true</c> ; yoksa, <c>false</c>.
        /// </value>
        bool IsDownloadFinished { get; }

        /// <summary>
        /// �ndirme h�z�n� getirir.
        /// </summary>
        /// <value>
        /// �ndirme h�z�.
        /// </value>
        double? DownloadRate { get; }

        /// <summary>
        /// �ndirme y�zdesini getirir.
        /// </summary>
        /// <value>
        /// �ndirme y�zdesi.
        /// </value>
        double DownloadProgress { get; }

        /// <summary>
        /// B�l�t indirici i�in ba�lang�� konumu.
        /// </summary>
        /// <value>
        /// Ba�lang�� konumu.
        /// </value>
        long StartPosition { get; }

        /// <summary>
        /// B�l�t indirici i�in biti� konumu.
        /// </summary>
        /// <value>
        /// Biti� konumu.
        /// </value>
        long EndPosition { get; }

        /// <summary>
        /// B�l�t indiricinin durumunu getirir.
        /// </summary>
        /// <value>
        /// B�l�t indirici durumu.
        /// </value>
        SegmentState State { get; set; }

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
        /// B�l�t�n indirme yapt��� adresini getirir.
        /// </summary>
        /// <value>
        /// �ndirme adresi.
        /// </value>
        Uri Uri { get; }

        /// <summary>
        /// �ndirmenin biti� zaman�.
        /// </summary>
        /// <value>
        /// �ndirme biti� zaman�.
        /// </value>
        DateTime? DownloadFinishDate { get; }
    }
}