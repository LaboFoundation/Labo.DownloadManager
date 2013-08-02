using Labo.DownloadManager.Segment;
using NUnit.Framework;

namespace Labo.DownloadManager.Tests
{
    [TestFixture]
    public class DownloadSegmentPositionsCalculatorTestFixture
    {
        public static object[] CalculateTestCases =
            {
                new object[] {200, 5, 5, 200,  new []{ new DownloadSegmentPositions(0, 199) }},
                new object[] {200, 5, 5, 150,  new []{ new DownloadSegmentPositions(0, 149) }},
                new object[] {200, 1, 1, 150,  new []{ new DownloadSegmentPositions(0, 149) }},
                new object[] {200, 4, 1, 200,  new []{ new DownloadSegmentPositions(0, 199) }},
                new object[] {200, 5, 1, 250,  new []{ new DownloadSegmentPositions(0, 249) }},
                new object[] {200, 5, 5, 1000, new []{ new DownloadSegmentPositions(0,   199), 
                                                       new DownloadSegmentPositions(200, 399), 
                                                       new DownloadSegmentPositions(400, 599), 
                                                       new DownloadSegmentPositions(600, 799),
                                                       new DownloadSegmentPositions(800, 999) }},
                new object[] {200, 5, 4, 1000, new []{ new DownloadSegmentPositions(0,   249), 
                                                       new DownloadSegmentPositions(250, 499), 
                                                       new DownloadSegmentPositions(500, 749), 
                                                       new DownloadSegmentPositions(750, 999) }},
                new object[] {200, 5, 1, 1000, new []{ new DownloadSegmentPositions(0,   999)}},
                new object[] {200, 5, 8, 1000, new []{ new DownloadSegmentPositions(0,   199), 
                                                       new DownloadSegmentPositions(200, 399), 
                                                       new DownloadSegmentPositions(400, 599), 
                                                       new DownloadSegmentPositions(600, 799),
                                                       new DownloadSegmentPositions(800, 999) }},
                new object[] {200, 5, 6, 500,  new []{ new DownloadSegmentPositions(0,   249), 
                                                       new DownloadSegmentPositions(250, 499) }},
                new object[] {200, 2, 6, 150,  new []{ new DownloadSegmentPositions(0,   149) }},
                new object[] {200, 5, 6, 1004, new []{ new DownloadSegmentPositions(0,   199), 
                                                       new DownloadSegmentPositions(200, 399), 
                                                       new DownloadSegmentPositions(400, 599), 
                                                       new DownloadSegmentPositions(600, 799),
                                                       new DownloadSegmentPositions(800, 1003) }}
            };

        [TestCaseSource("CalculateTestCases")]
        [Test]
        public void Calculate(int minimumSegmentSize, int maximumSegmentCount, int segmentCount, int fileSize, DownloadSegmentPositions[] expectedPositions)
        {
            DownloadSegmentPositionsCalculator downloadSegmentPositionsCalculator = new DownloadSegmentPositionsCalculator();
            DownloadSegmentPositions[] downloadSegmentPositions = downloadSegmentPositionsCalculator.Calculate(minimumSegmentSize, maximumSegmentCount, segmentCount, fileSize);

            Assert.AreEqual(expectedPositions.Length, downloadSegmentPositions.Length);

            for (int i = 0; i < expectedPositions.Length; i++)
            {
                Assert.AreEqual(expectedPositions[i].StartPosition, downloadSegmentPositions[i].StartPosition);
                Assert.AreEqual(expectedPositions[i].EndPosition,   downloadSegmentPositions[i].EndPosition);
            }
        }
    }
}
