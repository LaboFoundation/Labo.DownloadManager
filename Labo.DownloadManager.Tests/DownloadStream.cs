using System.IO;

namespace Labo.DownloadManager.Tests
{
    public sealed class DownloadStream : Stream
    {
        private readonly MemoryStream m_MemoryStream;

        public DownloadStream(byte[] data)
        {
            m_MemoryStream = new MemoryStream(data, false);
            TotalDownloads = 0;
        }

        public override bool CanRead
        {
            get { return m_MemoryStream.CanRead; }
        }

        public override bool CanSeek
        {
            get { return m_MemoryStream.CanSeek; }
        }

        public override bool CanWrite
        {
            get { return m_MemoryStream.CanWrite; }
        }

        public override void Flush()
        {
            m_MemoryStream.Flush();
        }

        public override long Length
        {
            get { return m_MemoryStream.Length; }
        }

        public override long Position
        {
            get
            {
                return m_MemoryStream.Position;
            }
            set
            {
                m_MemoryStream.Position = value;
            }
        }

        public long TotalDownloads { get; private set; }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int read = m_MemoryStream.Read(buffer, offset, count);
            TotalDownloads += read;
            return read;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return m_MemoryStream.Seek(offset, origin);
        }

        public override void SetLength(long value)
        {
            m_MemoryStream.SetLength(value);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            m_MemoryStream.Write(buffer, offset, count);
        }

        public byte[] ToArray()
        {
            return m_MemoryStream.ToArray();
        }
    }
}
