namespace Labo.DownloadManager
{
    using System;
    using System.Collections.Generic;

    using Labo.Common.Utils;
    using Labo.DownloadManager.EventAggregator;
    using Labo.DownloadManager.EventArgs;
    using Labo.DownloadManager.Protocol;
    using Labo.DownloadManager.Protocol.Providers;
    using Labo.DownloadManager.Settings;
    using Labo.DownloadManager.Streaming;

    /// <summary>
    /// The download helper class.
    /// </summary>
    public sealed class DownloadHelper
    {
        /// <summary>
        /// The download manager
        /// </summary>
        private readonly DownloadManager m_DownloadManager;

        /// <summary>
        /// Th event manager
        /// </summary>
        private readonly EventManager m_EventManager;

        /// <summary>
        /// The download manager
        /// </summary>
        public DownloadManager DownloadManager
        {
            get
            {
                return m_DownloadManager;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadHelper"/> class.
        /// </summary>
        /// <param name="downloadSettings">The download settings.</param>
        /// <param name="downloadStreamManager">The download stream manager.</param>
        public DownloadHelper(IDownloadSettings downloadSettings, IDownloadStreamManager downloadStreamManager)
        {
            INetworkProtocolProviderFactory networkProtocolProviderFactory = DownloadManagerRuntime.NetworkProtocolProviderFactory;
            INetworkProtocolProvider httpProtocolProvider = new HttpProtocolProvider(new WebRequestManager(new WebRequestFactory()));
            networkProtocolProviderFactory.RegisterProvider("http", httpProtocolProvider);
            networkProtocolProviderFactory.RegisterProvider("https", httpProtocolProvider);
           
            m_EventManager = new EventManager();
            m_DownloadManager = new DownloadManager(
                downloadSettings,
                downloadStreamManager,
                networkProtocolProviderFactory,
                m_EventManager);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DownloadHelper"/> class.
        /// </summary>
        public DownloadHelper()
            : this(new InMemoryDownloadSettings(200, 5, 8192, 5, 10, 800), new MemoryDownloadStreamManager())
        {
        }

        public void Start()
        {
            m_DownloadManager.Start();
        }

        public void PauseDownloadTask(Guid taskGuid)
        {
            m_DownloadManager.PauseDownloadTask(taskGuid);
        }

        public void StartDownloadTask(Guid taskGuid)
        {
            m_DownloadManager.StartDownloadTask(taskGuid);
        }

        public void Shutdown(bool waitAllDownloads)
        {
            m_DownloadManager.Shutdown(waitAllDownloads);
        }

        public Guid AddNewDownloadTask(DownloadTaskInfo downloadTaskInfo)
        {
            return m_DownloadManager.AddNewDownloadTask(downloadTaskInfo);
        }

        public Guid AddNewDownloadTask(Uri uri, string fileName, int segmentCount = 5)
        {
            return m_DownloadManager.AddNewDownloadTask(new DownloadTaskInfo(new DownloadFileInfo(uri, fileName, segmentCount), true));
        }

        public void OnDownloadFinished(Action<DownloadTaskFinishedEventMessage> action)
        {
            m_EventManager.EventSubscriber.RegisterConsumer(new ActionEventConsumer<DownloadTaskFinishedEventMessage>(action));
        }

        public IList<DownloadTaskStatistics> GetDownloadTaskStatistics()
        {
            return m_DownloadManager.GetDownloadTaskStatistics();
        }

        public DownloadTaskStatistics GetDownloadTaskStatistics(Guid guid)
        {
            return m_DownloadManager.GetDownloadTaskStatistics(guid);            
        }
    }
}
