namespace Labo.DownloadManager
{
    public sealed class DownloadFileInfo
    {
        public string Url { get; set; }

        public bool Authenticate { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string FileName { get; set; }

        public int SegmentCount { get; set; }

        public DownloadFileInfo(string url, string fileName, int segmentCount)
            : this(url, fileName, segmentCount, false, null, null)
        {
        }

        public DownloadFileInfo(string url, string fileName, int segmentCount, string userName, string password)
            : this(url, fileName, segmentCount, true, userName, password)
        {
        }

        public DownloadFileInfo(string url, string fileName, int segmentCount, bool authenticate, string userName, string password)
        {
            Url = url;
            FileName = fileName;
            SegmentCount = segmentCount;
            Authenticate = authenticate;
            UserName = userName;
            Password = password;
        }
    }
}
