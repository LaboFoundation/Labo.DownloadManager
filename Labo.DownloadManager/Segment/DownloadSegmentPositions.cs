namespace Labo.DownloadManager.Segment
{
    public sealed class DownloadSegmentPositions
    {
        public long StartPosition { get; private set; }

        public long EndPosition { get; private set; }

        public DownloadSegmentPositions(long startPosition, long endPosition)
        {
            EndPosition = endPosition;
            StartPosition = startPosition;
        }
    }
}