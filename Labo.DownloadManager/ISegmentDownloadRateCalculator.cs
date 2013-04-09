namespace Labo.DownloadManager
{
    public interface ISegmentDownloadRateCalculator
    {
        double? CalculateDownloadRate(long currentPosition);
    }
}
