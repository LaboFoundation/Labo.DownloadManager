using System.Globalization;
using System.IO;

namespace Labo.DownloadManager
{
    public sealed class DefaultFileNameCorrector : IFileNameCorrector
    {
        public string GetFileName(string fileName)
        {
            if (File.Exists(fileName))
            {
                int count = 1;

                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                string fileExtension = Path.GetExtension(fileName);

                string newFileName;

                do
                {
                    newFileName = string.Format(CultureInfo.CurrentCulture, "{0}{1}({2}){3}", Path.GetDirectoryName(fileName), fileNameWithoutExtension, count++, fileExtension);
                }
                while (File.Exists(newFileName));

                return newFileName;
            }
            return fileName;
        }
    }
}