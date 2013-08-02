using System.IO;
namespace Labo.DownloadManager.Segment
{
    public sealed class SegmentWriter : ISegmentWriter
    {
        private readonly Stream m_Stream;

        public SegmentWriter(Stream stream)
        {
            m_Stream = stream;
        }

        public void Write(long startPosition, byte[] buffer, int count)
        {
            lock (m_Stream)
            {
                m_Stream.Position = startPosition;
                m_Stream.Write(buffer, 0, count);
            }
        }
    }
}