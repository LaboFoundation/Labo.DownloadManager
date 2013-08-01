namespace Labo.DownloadManager.Segment
{
    public sealed class SegmentWriter : ISegmentWriter
    {
        private readonly System.IO.Stream m_Stream;

        public SegmentWriter(System.IO.Stream stream)
        {
            m_Stream = stream;
        }

        public void Write(long startPosition, byte[] buffer, int count)
        {
            m_Stream.Position = startPosition;
            m_Stream.Write(buffer, 0, count);
        }
    }
}