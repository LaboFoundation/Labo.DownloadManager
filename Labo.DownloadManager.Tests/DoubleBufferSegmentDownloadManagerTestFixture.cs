using Labo.DownloadManager.Segment;

namespace Labo.DownloadManager.Tests
{
    public class DoubleBufferSegmentDownloadManagerTestFixture : BaseSegmentDownloadManagerTestFixture
    {
        protected override ISegmentDownloadTask CreateSegmentDownloadTask(int bufferSize, ISegmentDownloader segmentDownloader, ISegmentWriter segmentWriter)
        {
            return new DoubleBufferSegmentDownloadTask(bufferSize, segmentDownloader, segmentWriter);
        }
    }
}