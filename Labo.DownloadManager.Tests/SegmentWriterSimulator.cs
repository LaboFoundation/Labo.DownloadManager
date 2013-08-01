using System.Threading;
using Labo.DownloadManager.Segment;

namespace Labo.DownloadManager.Tests
{
    public sealed class SegmentWriterSimulator : ISegmentWriter
    {
        private readonly ISegmentWriter m_SegmentWriter;

        public void Write(long startPosition, byte[] buffer, int count)
        {
            Thread.Sleep(1000);
            m_SegmentWriter.Write(startPosition, buffer, count);
        }

        public SegmentWriterSimulator(ISegmentWriter segmentWriter)
        {
            m_SegmentWriter = segmentWriter;
        }
    }
}