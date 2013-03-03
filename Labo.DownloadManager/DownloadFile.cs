namespace Labo.DownloadManager
{
    public sealed class DownloadFile
    {
        public string Url { get; set; }

        public bool Authenticate { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }
    }
}
