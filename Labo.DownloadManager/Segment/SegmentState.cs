namespace Labo.DownloadManager.Segment
{
    public enum SegmentState
    {
        Idle = 0,
        Connecting,
        Downloading,
        Paused,
        Finished,
        Error,
    }
}