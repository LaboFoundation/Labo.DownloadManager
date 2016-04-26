namespace Labo.DownloadManager.Segment
{
    using System;

    /// <summary>
    /// Bölüt indirici arayüzü.
    /// </summary>
    public interface ISegmentDownloader : ISegmentDownloaderInfo
    {
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
        int Download(byte[] buffer);

        /// <summary>
        /// Bölüt indiricinin mevcut konumunu verilen deðer kadar arttýrýr.
        /// </summary>
        /// <param name="size">Artýþ deðeri.</param>
        void IncreaseCurrentPosition(int size);

        /// <summary>
        /// Alýnan hatayý atar.
        /// </summary>
        /// <param name="exception">Hata.</param>
        void SetError(Exception exception);

        /// <summary>
        /// Ýndirme bitiþ zamanýný atar.
        /// </summary>
        /// <param name="date">Bitiþ zamaný.</param>
        void SetDownloadFinishDate(DateTime date);

        /// <summary>
        /// Ýndirme kaynaðýný günceller.
        /// </summary>
        void RefreshDownloadStream();

        void CalculateCurrentDownloadRate(long currentPosition);
    }
}