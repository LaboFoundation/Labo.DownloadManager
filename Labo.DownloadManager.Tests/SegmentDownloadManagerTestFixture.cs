using Labo.DownloadManager.Segment;

namespace Labo.DownloadManager.Tests
{
    public class SegmentDownloadManagerTestFixture : BaseSegmentDownloadManagerTestFixture
    {
        protected override ISegmentDownloadTask CreateSegmentDownloadTask(int bufferSize, ISegmentDownloader segmentDownloader, ISegmentWriter segmentWriter)
        {
            return new SegmentDownloadTask(bufferSize, segmentDownloader, segmentWriter);
        }
    }
}