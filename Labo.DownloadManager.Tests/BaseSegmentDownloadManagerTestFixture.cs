using System.Collections.Generic;
using System.IO;
using System.Linq;
using Labo.DownloadManager.Segment;
using NUnit.Framework;

namespace Labo.DownloadManager.Tests
{
    [TestFixture]
    public abstract class BaseSegmentDownloadManagerTestFixture
    {
        public static object[] DownloadTestCases =
            {
                new object[] {new byte[] {0,  1,  2,  3,  4,  5,  6,  7,  8,  9,  10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 
                                          25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49}, 5, new []
                    {
                        new DownloadSegmentPositions(0, 9), 
                        new DownloadSegmentPositions(10, 19),
                        new DownloadSegmentPositions(20, 29), 
                        new DownloadSegmentPositions(30, 39),
                        new DownloadSegmentPositions(40, 49)
                    }},
                new object[] {new byte[] {0,  1,  2,  3,  4,  5,  6,  7,  8,  9,  10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 
                                          25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49}, 6, new []
                    {
                        new DownloadSegmentPositions(0, 9), 
                        new DownloadSegmentPositions(10, 19),
                        new DownloadSegmentPositions(20, 29), 
                        new DownloadSegmentPositions(30, 39),
                        new DownloadSegmentPositions(40, 49)
                    }}
            };

        [TestCaseSource(typeof(BaseSegmentDownloadManagerTestFixture), "DownloadTestCases")]
        [Test]
        public void Download(byte[] data, int bufferSize, DownloadSegmentPositions[] segmentPositionInfos)
        {
            BeforeDownload();

            DownloadStream inputStream = new DownloadStream(data);

            IList<ISegmentDownloadTask> segmentDownloadTasks = new List<ISegmentDownloadTask>(segmentPositionInfos.Length);
            IList<DownloadStream> downloadStreams = new List<DownloadStream>(segmentPositionInfos.Length);
            MemoryStream outputStream = new MemoryStream();
            for (int i = 0; i < segmentPositionInfos.Length; i++)
            {
                DownloadSegmentPositions segmentPosition = segmentPositionInfos[i];
                byte[] dataPart = data.Skip((int) segmentPosition.StartPosition).Take((int)(segmentPosition.EndPosition - segmentPosition.StartPosition + 1)).ToArray();
                DownloadStream downloadStream = new DownloadStream(dataPart);
                segmentDownloadTasks.Add(CreateSegmentDownloadTask(bufferSize, CreateSegmentDownloader(downloadStream, segmentPosition), CreateSegmentWriter(outputStream)));
                downloadStreams.Add(downloadStream);
            }

            SegmentDownloadManager segmentDownloadManager = new SegmentDownloadManager(new SegmentDownloadTaskCollection(segmentDownloadTasks));
            segmentDownloadManager.Start();
            segmentDownloadManager.Finish(true);

            AfterDownload();

            long totalDownloads = downloadStreams.Sum(x => x.TotalDownloads);
            Assert.AreEqual(data.Length, totalDownloads);
            Assert.AreEqual(inputStream.ToArray().Take(data.Length).ToArray(), outputStream.ToArray().Take(data.Length).ToArray());
            Assert.AreEqual(inputStream.ToArray(), outputStream.ToArray());
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
