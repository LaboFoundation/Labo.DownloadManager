using System;

namespace Labo.DownloadManager
{
    public sealed class RemoteFileInfo
    {
        public string MimeType { get; set; }

        public bool AcceptRanges { get; set; }

        public long FileSize { get; set; }

        public DateTime LastModified { get; set; }

        public string FileName { get; set; }
    }
}