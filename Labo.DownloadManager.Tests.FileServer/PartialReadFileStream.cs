using System;
using System.IO;

namespace Labo.DownloadManager.Tests.FileServer
{
    internal class PartialReadFileStream : Stream
    {
        private readonly long m_Start;
        private readonly long m_End;
        private long m_Position;
        private readonly Stream m_FileStream;

        public PartialReadFileStream(Stream fileStream, long start, long end)
        {
            m_Start = start;
            m_Position = start;
            m_End = end;
            m_FileStream = fileStream;

            if (start > 0)
            {
                m_FileStream.Seek(start, SeekOrigin.Begin);
            }
        }

        public override void Flush()
        {
            m_FileStream.Flush();
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (origin == SeekOrigin.Begin)
            {
                m_Position = m_Start + offset;
                return m_FileStream.Seek(m_Start + offset, origin);
            }
            else if (origin == SeekOrigin.Current)
            {
                m_Position += offset;
                return m_FileStream.Seek(m_Position + offset, origin);
            }
            else
            {
                throw new NotImplementedException("Seeking from SeekOrigin.End is not implemented");
            }
        }

        public override void SetLength(long value)
        {
            throw new System.NotImplementedException();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            int byteCountToRead = count;
            if (m_Position + count > m_End)
            {
                byteCountToRead = (int)(m_End - m_Position) + 1;
            }
            var result = m_FileStream.Read(buffer, offset, byteCountToRead);
            m_Position += byteCountToRead;
            return result;
        }

        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            int byteCountToRead = count;
            if (m_Position + count > m_End)
            {
                byteCountToRead = (int)(m_End - m_Position);
            }
            var result = m_FileStream.BeginRead(buffer, offset,
                                                count, (s) =>
                                                    {
                                                        m_Position += byteCountToRead;
                                                        callback(s);
                                                    }, state);
            return result;
        }

        public override int EndRead(IAsyncResult asyncResult)
        {
            return m_FileStream.EndRead(asyncResult);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            throw new System.NotImplementedException();
        }

        public override int ReadByte()
        {
            int result = m_FileStream.ReadByte();
            m_Position++;
            return result;
        }

        public override void WriteByte(byte value)
        {
            throw new NotImplementedException();
        }

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override long Length
        {
            get { return m_End - m_Start; }
        }

        public override long Position
        {
            get { return m_Position; }
            set
            {
                m_Position = value;
                m_FileStream.Seek(m_Position, SeekOrigin.Begin);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                m_FileStream.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}