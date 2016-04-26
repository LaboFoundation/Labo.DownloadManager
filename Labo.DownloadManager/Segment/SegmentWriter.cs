namespace Labo.DownloadManager.Segment
{
    using System;
    using System.IO;

    public sealed class SegmentWriter : ISegmentWriter
    {
        private Stream m_Stream;

        public void SetStream(Stream stream)
        {
            m_Stream = stream;
        }

        public void Write(long startPosition, byte[] buffer, int count)
        {
            lock (this)
            {
                if (m_Stream == null)
                {
                    throw new InvalidOperationException("stream is not set");
                }

                m_Stream.Position = startPosition;
                m_Stream.Write(buffer, 0, count);
            }
        }
    }
}