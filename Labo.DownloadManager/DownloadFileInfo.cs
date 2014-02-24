namespace Labo.DownloadManager
{
    using System;

    public sealed class DownloadFileInfo
    {
        public Uri Uri { get; set; }

        public bool Authenticate { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string FileName { get; set; }

        public int SegmentCount { get; set; }

        public DownloadFileInfo(Uri uri, string fileName, int segmentCount)
            : this(uri, fileName, segmentCount, false, null, null)
        {
        }

        public DownloadFileInfo(Uri uri, string fileName, int segmentCount, string userName, string password)
            : this(uri, fileName, segmentCount, true, userName, password)
        {
        }

        public DownloadFileInfo(Uri uri, string fileName, int segmentCount, bool authenticate, string userName, string password)
        {
            Uri = uri;
            FileName = fileName;
            SegmentCount = segmentCount;
            Authenticate = authenticate;
            UserName = userName;
            Password = password;
        }
    }
}
