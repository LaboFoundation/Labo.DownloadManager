namespace Labo.DownloadManager.Tests
{
    using System.IO;
    using System.Linq;

    using NUnit.Framework;

    public sealed class TestOutputStream : Stream
    {
        private readonly byte[] m_Data;

        private long m_Length;

        private long m_Position;

        public TestOutputStream(byte[] data)
        {
            m_Data = data;
            TotalDownloads = 0;
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override void Flush()
        {
        }

        public override long Length
        {
            get { return m_Length; }
        }

        public override long Position
        {
            get
            {
                return m_Position;
            }
            set
            {
                m_Position = value;
            }
        }

        public long TotalDownloads { get; private set; }

        public override void SetLength(long value)
        {
            m_Length = value;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            byte[] data = m_Data.Skip((int)Position).Take(count).ToArray();

            Assert.IsTrue(data.SequenceEqual(buffer.Take(count).ToArray()));
        }

        public byte[] ToArray()
        {
            return m_Data;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return -1;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return -1;
        }
    }
}