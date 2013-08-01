namespace Labo.DownloadManager.Segment
{
    public interface ISegmentDownloadRateCalculator
    {
        double? CalculateDownloadRate(long currentPosition);
    }
}
