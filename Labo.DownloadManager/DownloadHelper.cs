using System;

using Labo.DownloadManager.EventAggregator;
using Labo.DownloadManager.EventArgs;
using Labo.DownloadManager.Protocol;
using Labo.DownloadManager.Protocol.Providers;
using Labo.DownloadManager.Settings;
using Labo.DownloadManager.Streaming;

namespace Labo.DownloadManager
{
    using System.Collections.Generic;

    public sealed class DownloadHelper
    {
        private readonly DownloadManager m_DownloadManager;
        private readonly EventManager m_EventManager;

        public DownloadHelper()
        {
            NetworkProtocolProviderFactory networkProtocolProviderFactory = new NetworkProtocolProviderFactory(new UrlProtocolParser());
            HttpProtocolProvider httpProtocolProvider = new HttpProtocolProvider(new WebRequestManager(new WebRequestFactory()));
            networkProtocolProviderFactory.RegisterProvider("http", httpProtocolProvider);
            networkProtocolProviderFactory.RegisterProvider("https", httpProtocolProvider);
            m_EventManager = new EventManager();
            m_DownloadManager = new DownloadManager(
                new InMemoryDownloadSettings(200, 5, 8096, 5, 5),
                new MemoryDownloadStreamManager(),
                networkProtocolProviderFactory,
                m_EventManager);
        }

        public void Start()
        {
            m_DownloadManager.Start();
        }

        public void Shutdown(bool waitAllDownloads)
        {
            m_DownloadManager.Shutdown(waitAllDownloads);
        }

        public Guid AddNewDownloadTask(DownloadTaskInfo downloadTaskInfo)
        {
            return m_DownloadManager.AddNewDownloadTask(downloadTaskInfo);
        }

        public void AddNewDownloadTask(string url, string fileName)
        {
            m_DownloadManager.AddNewDownloadTask(new DownloadTaskInfo(new DownloadFileInfo(url, fileName, 5), true));
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
