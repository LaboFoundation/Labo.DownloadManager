namespace Labo.DownloadManager
{
    public interface ISegmentWriter
    {
        void Write(long startPosition, byte[] buffer, int count);
    }
}