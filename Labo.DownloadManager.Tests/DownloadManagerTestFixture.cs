using System.IO;
using System.Text;
using Labo.DownloadManager.EventAggregator;
using Labo.DownloadManager.EventArgs;
using Labo.DownloadManager.Protocol;
using Labo.DownloadManager.Protocol.Providers;
using Labo.DownloadManager.Settings;
using Labo.DownloadManager.Streaming;
using NUnit.Framework;

namespace Labo.DownloadManager.Tests
{
    using System;

    [TestFixture]
    public class DownloadManagerTestFixture
    {
        [Test, Ignore("written to see what is happening")]
        public void Download()
        {
            NetworkProtocolProviderFactory networkProtocolProviderFactory = new NetworkProtocolProviderFactory(new UrlProtocolParser());
            HttpProtocolProvider httpProtocolProvider = new HttpProtocolProvider(new WebRequestManager(new WebRequestFactory()));
            networkProtocolProviderFactory.RegisterProvider("http", httpProtocolProvider);
            networkProtocolProviderFactory.RegisterProvider("https", httpProtocolProvider);

            EventManager eventManager = new EventManager();
            DownloadManager downloadManager = new DownloadManager(
                new InMemoryDownloadSettings(200, 5, 8096, 5, 5, 0),
                new MemoryDownloadStreamManager(),
                networkProtocolProviderFactory,
                eventManager);
            downloadManager.Start();

            downloadManager.AddNewDownloadTask(new DownloadTaskInfo(new DownloadFileInfo(new Uri("https://androidnetworktester.googlecode.com/files/1mb.txt"), "1mb.txt", 5), true));
            downloadManager.AddNewDownloadTask(new DownloadTaskInfo(new DownloadFileInfo(new Uri("http://127.0.0.1:8552/1mb.txt"), "1mb.txt", 5), true));

            eventManager.EventSubscriber.RegisterConsumer(new ActionEventConsumer<DownloadTaskFinishedEventMessage>(x =>
                {
                    string txt = Encoding.UTF8.GetString(((MemoryStream)x.DownloadStream).ToArray());
                    txt.ToString();
                }));

            downloadManager.Shutdown(true);
        }
    }
}