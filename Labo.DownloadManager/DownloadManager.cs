using Labo.DownloadManager.EventAggregator;
using Labo.DownloadManager.Protocol;
using Labo.DownloadManager.Settings;
using Labo.DownloadManager.Streaming;

namespace Labo.DownloadManager
{
    public sealed class DownloadManager
    {
        private readonly IDownloadTaskList m_DownloadTaskList;
        private readonly DownloadTaskQueue m_DownloadTaskQueue;
        private readonly IDownloadSettings m_DownloadSettings;
        private readonly IDownloadStreamManager m_LocalFileDownloadStreamManager;
        private readonly INetworkProtocolProviderFactory m_NetworkProtocolProviderFactory;
        private readonly IEventManager m_EventManager;

        public DownloadManager(IDownloadSettings downloadSettings, IDownloadStreamManager downloadStreamManager, INetworkProtocolProviderFactory networkProtocolProviderFactory, IEventManager eventManager)
        {
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

        public void AddNewDownloadTask(DownloadTaskInfo downloadTaskInfo)
        {
            AddNewDownloadTask(new DownloadTask(m_NetworkProtocolProviderFactory,
                                                DownloadManagerRuntime.DownloadSegmentPositionsCalculator,
                                                m_LocalFileDownloadStreamManager,
                                                m_DownloadSettings,
                                                downloadTaskInfo.DownloadFileInfo, 
                                                m_EventManager),
                                                downloadTaskInfo.StartImmediately);
        }

        private void AddNewDownloadTask(IDownloadTask downloadTask, bool startImmediately)
        {
            m_DownloadTaskList.Add(downloadTask);

            if (startImmediately)
            {
                m_DownloadTaskQueue.EnqueueDownloadTask(downloadTask);
            }
        }
    }
}
