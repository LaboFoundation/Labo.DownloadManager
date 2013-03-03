namespace Labo.DownloadManager
{
    public interface ILocalFileAllocator
    {
        LocalFileInfo AllocateFile(string fileName, long fileSize);
    }
}
