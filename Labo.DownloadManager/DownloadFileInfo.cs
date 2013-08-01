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
    }
}
