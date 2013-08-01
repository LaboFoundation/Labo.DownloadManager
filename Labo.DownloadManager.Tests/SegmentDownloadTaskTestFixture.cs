using Labo.DownloadManager.Segment;

namespace Labo.DownloadManager.Tests
{
    public class SegmentDownloadTaskTestFixture : BaseSegmentDownloadTaskTestFixture
    {
        protected override ISegmentDownloadTask CreateSegmentDownloadTask(int bufferSize, ISegmentDownloader segmentDownloader, ISegmentWriter segmentWriter)
        {
            return new SegmentDownloadTask(bufferSize, segmentDownloader, segmentWriter);
        }
    }
}