using Labo.DownloadManager.Segment;

namespace Labo.DownloadManager.Tests
{
    public class DoubleBufferSegmentDownloadTaskTestFixture : BaseSegmentDownloadTaskTestFixture
    {
        protected override ISegmentDownloadTask CreateSegmentDownloadTask(int bufferSize, ISegmentDownloader segmentDownloader, ISegmentWriter segmentWriter)
        {
            return new DoubleBufferSegmentDownloadTask(bufferSize, 0, 0, segmentDownloader, segmentWriter);
        }
    }
}