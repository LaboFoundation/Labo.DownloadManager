namespace Labo.DownloadManager.Segment
{
    using System;

    /// <summary>
    /// B�l�t indirici aray�z�.
    /// </summary>
    public interface ISegmentDownloader : ISegmentDownloaderInfo
    {
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
        int Download(byte[] buffer);

        /// <summary>
        /// B�l�t indiricinin mevcut konumunu verilen de�er kadar artt�r�r.
        /// </summary>
        /// <param name="size">Art�� de�eri.</param>
        void IncreaseCurrentPosition(int size);

        /// <summary>
        /// Al�nan hatay� atar.
        /// </summary>
        /// <param name="exception">Hata.</param>
        void SetError(Exception exception);

        /// <summary>
        /// �ndirme biti� zaman�n� atar.
        /// </summary>
        /// <param name="date">Biti� zaman�.</param>
        void SetDownloadFinishDate(DateTime date);

        /// <summary>
        /// �ndirme kayna��n� g�nceller.
        /// </summary>
        void RefreshDownloadStream();

        void CalculateCurrentDownloadRate(long currentPosition);
    }
}