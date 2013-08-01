namespace Labo.DownloadManager.Segment
{
    public interface ISegmentWriter
    {
        void Write(long startPosition, byte[] buffer, int count);
    }
}