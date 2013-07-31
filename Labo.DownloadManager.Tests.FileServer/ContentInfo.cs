namespace Labo.DownloadManager.Tests.FileServer
{
    public sealed class ContentInfo
    {
        public long From { get; set; }

        public long To { get; set; }

        public bool IsPartial { get; set; }

        public long Length { get; set; }
    }
}
