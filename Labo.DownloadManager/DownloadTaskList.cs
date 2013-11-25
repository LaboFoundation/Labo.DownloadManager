namespace Labo.DownloadManager
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class DownloadTaskList : IDownloadTaskList
    {
        private readonly ConcurrentDictionary<Guid, IDownloadTask> m_DownloadTasks;

        public DownloadTaskList()
        {
            m_DownloadTasks = new ConcurrentDictionary<Guid, IDownloadTask>();
        }

        public Guid Add(IDownloadTask downloadTask)
        {
            Guid guid = Guid.NewGuid();
            m_DownloadTasks.TryAdd(guid, downloadTask);

            return guid;
        }

        public IList<DownloadTaskStatistics> GetDownloadTaskStatistics()
        {
            return m_DownloadTasks.Select(x => x.Value.GetDownloadTaskStatistics()).ToList();
        }

        public DownloadTaskStatistics GetDownloadTaskStatistics(Guid guid)
        {
            return m_DownloadTasks[guid].GetDownloadTaskStatistics();
        }
    }
}
