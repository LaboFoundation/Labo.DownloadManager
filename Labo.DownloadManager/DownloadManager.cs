namespace Labo.DownloadManager
{
    using System;
    using System.Collections.Generic;

    using Labo.DownloadManager.EventAggregator;
    using Labo.DownloadManager.Protocol;
    using Labo.DownloadManager.Settings;
    using Labo.DownloadManager.Streaming;

    public sealed class DownloadManager
    {
        private readonly IDownloadTaskList m_DownloadTaskList;
        private readonly DownloadTaskQueue m_DownloadTaskQueue;
        private readonly IDownloadSettings m_DownloadSettings;
        private readonly IDownloadStreamManager m_LocalFileDownloadStreamManager;
        private readonly INetworkProtocolProviderFactory m_NetworkProtocolProviderFactory;
        private readonly IEventManager m_EventManager;

        public DownloadManager(IDownloadSettings downloadSettings, 
                               IDownloadStreamManager downloadStreamManager, 
                               INetworkProtocolProviderFactory networkProtocolProviderFactory,
                               IEventManager eventManager)
        {
            if (downloadSettings == null)
            {
                throw new ArgumentNullException("downloadSettings");
            }

            m_DownloadTaskList = new DownloadTaskList();
            m_DownloadTaskQueue = new DownloadTaskQueue(downloadSettings.MaximumConcurrentDownloads);
            m_DownloadSettings = downloadSettings;
            m_LocalFileDownloadStreamManager = downloadStreamManager;
            m_NetworkProtocolProviderFactory = networkProtocolProviderFactory;
            m_EventManager = eventManager;
        }

        public void Start()
        {
            m_DownloadTaskQueue.Start();
        }

        public void Shutdown(bool waitForDownloads)
        {
            m_DownloadTaskQueue.Shutdown(waitForDownloads);
        }

        public IList<DownloadTaskStatistics> GetDownloadTaskStatistics()
        {
            return m_DownloadTaskList.GetDownloadTaskStatistics();
        }

        public DownloadTaskStatistics GetDownloadTaskStatistics(Guid guid)
        {
            return m_DownloadTaskList.GetDownloadTaskStatistics(guid);
        }

        public Guid AddNewDownloadTask(DownloadTaskInfo downloadTaskInfo)
        {
            if (downloadTaskInfo == null)
            {
                throw new ArgumentNullException("downloadTaskInfo");
            }

            return AddNewDownloadTask(new DownloadTask(m_NetworkProtocolProviderFactory,
                                                DownloadManagerRuntime.DownloadSegmentPositionsCalculator,
                                                m_LocalFileDownloadStreamManager,
                                                m_DownloadSettings,
                                                downloadTaskInfo.DownloadFileInfo, 
                                                m_EventManager),
                                                downloadTaskInfo.StartImmediately);
        }

        private Guid AddNewDownloadTask(IDownloadTask downloadTask, bool startImmediately)
        {
            Guid guid = m_DownloadTaskList.Add(downloadTask);

            if (startImmediately)
            {
                m_DownloadTaskQueue.EnqueueDownloadTask(downloadTask);
            }

            return guid;
        }
    }
}
