namespace Labo.DownloadManager.Segment
{
    public interface IDownloadSegmentPositionsCalculator
    {
        DownloadSegmentPositions[] Calculate(int minimumSegmentSize, int maximumSegmentCount, int segmentCount, long fileSize);
    }
}