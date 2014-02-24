namespace Labo.DownloadManager.Protocol
{
    using System;

    public interface IUrlProtocolParser
    {
        string Parse(Uri uri);
    }
}