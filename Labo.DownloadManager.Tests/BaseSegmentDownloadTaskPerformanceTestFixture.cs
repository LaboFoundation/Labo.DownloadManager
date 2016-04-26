using System;
using System.Diagnostics;
using System.IO;
using Labo.DownloadManager.Segment;

namespace Labo.DownloadManager.Tests
{
    public abstract class BaseSegmentDownloadTaskPerformanceTestFixture : BaseSegmentDownloadTaskTestFixture
    {
        private Stopwatch m_Stopwatch;

        protected override ISegmentWriter CreateSegmentWriter(Stream outputStream)
        {
            SegmentWriter segmentWriter = new SegmentWriter();
            segmentWriter.SetStream(outputStream);

            return new SegmentWriterSimulator(segmentWriter);
        }

        protected override ISegmentDownloader CreateSegmentDownloader(Stream inputStream, DownloadSegmentPositions downloadSegmentPositions)
        {
            return new SegmentDownloaderSimulator(new SegmentDownloader(inputStream, downloadSegmentPositions, new SegmentDownloadRateCalculator()));
        }

        protected override void BeforeDownload()
        {
            m_Stopwatch = new Stopwatch();
            m_Stopwatch.Start();
        }

        protected override void AfterDownload()
        {
            m_Stopwatch.Stop();

            Console.WriteLine(m_Stopwatch.Elapsed);
        }
    }
}