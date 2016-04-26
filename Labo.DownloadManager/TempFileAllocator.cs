using System;
using System.IO;

namespace Labo.DownloadManager
{
    public sealed class TempFileAllocator : ILocalFileAllocator
    {
        private readonly IFileNameCorrector m_FileNameCorrector;

        public TempFileAllocator(IFileNameCorrector fileNameCorrector)
        {
            m_FileNameCorrector = fileNameCorrector;
        }

        public LocalFileInfo AllocateFile(string fileName, long fileSize)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException("fileName");
            }

            if (fileSize < 0)
            {
                throw new ArgumentOutOfRangeException("fileSize");
            }

            string fileDirectory = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(fileDirectory))
            {
                Directory.CreateDirectory(fileDirectory);
            }

            fileName = m_FileNameCorrector.GetFileName(fileName);

            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                fs.SetLength(fileSize);
            }

            return new LocalFileInfo
                {
                    FileName = fileName
                };
        }
    }
}