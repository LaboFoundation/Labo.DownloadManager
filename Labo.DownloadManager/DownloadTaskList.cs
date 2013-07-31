using System;
using System.Collections.Concurrent;

namespace Labo.DownloadManager
{
    internal sealed class DownloadTaskList : IDownloadTaskList
    {
        private readonly ConcurrentDictionary<Guid, IDownloadTask> m_DownloadTasks;

        public DownloadTaskList()
        {
            m_DownloadTasks = new ConcurrentDictionary<Guid, IDownloadTask>();
        }

        public void Add(IDownloadTask downloadTask)
        {
            m_DownloadTasks.TryAdd(Guid.NewGuid(), downloadTask);
        }
    }
}
