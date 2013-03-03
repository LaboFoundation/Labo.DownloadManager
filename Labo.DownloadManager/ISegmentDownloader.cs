namespace Labo.DownloadManager
{
    public interface ISegmentDownloader
    {
        long CurrentPosition { get; }

        bool IsDownloadFinished { get; }

        int Download(byte[] buffer);

        void IncreaseCurrentPosition(int size);
    }
}