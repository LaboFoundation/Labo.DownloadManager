using System.IO;
using System.Linq;
using Labo.DownloadManager.Segment;
using NUnit.Framework;
namespace Labo.DownloadManager.Tests
{
    [TestFixture]
    public abstract class BaseSegmentDownloadTaskTestFixture
    {
        public static object[] DownloadTestCases =
            {
                new object[] {new byte[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20}, 5, 20},
                new object[] {new byte[] {0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20}, 6, 15}
            };

        [TestCaseSource(typeof(BaseSegmentDownloadTaskTestFixture), "DownloadTestCases")]
        [Test]
        public void Download(byte[] data, int bufferSize, int endPosition)
        {
            BeforeDownload();

            DownloadStream inputStream = new DownloadStream(data);

            MemoryStream outputStream = new MemoryStream();
            DownloadSegmentPositions downloadSegmentPositions = new DownloadSegmentPositions {StartPosition = 0, EndPosition = endPosition};

            ISegmentDownloadTask task = CreateSegmentDownloadTask(bufferSize, CreateSegmentDownloader(inputStream, downloadSegmentPositions), CreateSegmentWriter(outputStream));
            task.Download();

            AfterDownload();

            Assert.AreEqual(endPosition, inputStream.TotalDownloads);
            Assert.AreEqual(inputStream.ToArray().Take(endPosition).ToArray(), outputStream.ToArray().Take(endPosition).ToArray());
        }

        protected virtual ISegmentWriter CreateSegmentWriter(Stream outputStream)
        {
            return new SegmentWriter(outputStream);
        }

        protected virtual ISegmentDownloader CreateSegmentDownloader(Stream inputStream, DownloadSegmentPositions downloadSegmentPositions)
        {
            return new SegmentDownloader(inputStream, downloadSegmentPositions, new SegmentDownloadRateCalculator(downloadSegmentPositions.StartPosition));
        }

        protected virtual void BeforeDownload()
        {
        }

        protected virtual void AfterDownload()
        {
        }

        protected abstract ISegmentDownloadTask CreateSegmentDownloadTask(int bufferSize, ISegmentDownloader segmentDownloader, ISegmentWriter segmentWriter);
    }
}
