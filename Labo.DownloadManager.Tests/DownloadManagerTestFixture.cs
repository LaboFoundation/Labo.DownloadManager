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
    [TestFixture]
    public class DownloadManagerTestFixture
    {
        [Test, Ignore("written to see what is happening")]
        public void Download()
        {
            MemoryStream stream = new MemoryStream();
            NetworkProtocolProviderFactory networkProtocolProviderFactory = new NetworkProtocolProviderFactory(new UrlProtocolParser());
            HttpProtocolProvider httpProtocolProvider = new HttpProtocolProvider(new WebRequestManager(new WebRequestFactory()));
            networkProtocolProviderFactory.RegisterProvider("http", httpProtocolProvider);
            networkProtocolProviderFactory.RegisterProvider("https", httpProtocolProvider);

            EventManager eventManager = new EventManager();
            DownloadManager downloadManager = new DownloadManager(
                new InMemoryDownloadSettings(200, 5, 8096, 5),
                new MemoryDownloadStreamManager(stream),
                networkProtocolProviderFactory,
                eventManager);
            downloadManager.Start();

            downloadManager.AddNewDownloadTask(new DownloadTaskInfo(new DownloadFileInfo
                {
                    Url = "https://androidnetworktester.googlecode.com/files/1mb.txt",
                    SegmentCount = 5
                }, true));

            downloadManager.AddNewDownloadTask(new DownloadTaskInfo(new DownloadFileInfo
                {
                    Url = "https://androidnetworktester.googlecode.com/files/1mb.txt",
                    SegmentCount = 5
                }, true));
            eventManager.EventSubscriber.RegisterConsumer(new ActionEventConsumer<DownloadTaskFinishedEventMessage>(x =>
                {
                    string txt = Encoding.UTF8.GetString(stream.ToArray());
                    txt.ToString();
                }));

            downloadManager.Shutdown(true);
        }
    }
}