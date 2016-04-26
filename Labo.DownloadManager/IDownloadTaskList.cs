namespace Labo.DownloadManager
{
    using System;
    using System.Collections.Generic;

    public interface IDownloadTaskList
    {
        Guid Add(IDownloadTask downloadTask);

        IList<DownloadTaskStatistics> GetDownloadTaskStatistics();

        DownloadTaskStatistics GetDownloadTaskStatistics(Guid guid);

        IDownloadTask GetDownloadTaskByGuid(Guid guid);
    }
}