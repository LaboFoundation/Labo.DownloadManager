using System.Diagnostics;
using System.IO;
using System.Text;
using Labo.DownloadManager.EventAggregator;
using Labo.DownloadManager.EventArgs;
using Labo.DownloadManager.Protocol;
using Labo.DownloadManager.Protocol.Providers;
using Labo.DownloadManager.Settings;
using NUnit.Framework;

namespace Labo.DownloadManager.Tests
{
    [TestFixture]
    public class DownloadTaskTestFixture
    {
        [Test]
        public void Download()
        {
            MemoryStream stream = new MemoryStream();
            NetworkProtocolProviderFactory networkProtocolProviderFactory = new NetworkProtocolProviderFactory(new UrlProtocolParser());
            HttpProtocolProvider httpProtocolProvider = new HttpProtocolProvider(new WebRequestManager(new WebRequestFactory()));
            networkProtocolProviderFactory.RegisterProvider("http", httpProtocolProvider);
            networkProtocolProviderFactory.RegisterProvider("https", httpProtocolProvider);

            DownloadTask downloadTask = new DownloadTask(networkProtocolProviderFactory,
                                                          new DownloadSegmentPositionsCalculator(),
                                                          new MemoryDownloadStreamManager(stream),
                                                          new InMemoryDownloadSettings(200, 5, 8096),
                                                          new DownloadFileInfo
                                                              {
                                                                  Url = "https://androidnetworktester.googlecode.com/files/1mb.txt",
                                                                  SegmentCount = 5
                                                              }, new EventManager());

            downloadTask.EventManager.EventSubscriber.RegisterConsumer(new ActionEventConsumer<DownloadTaskFinishedEventMessage>(x =>
                {
                    string txt = Encoding.UTF8.GetString(stream.ToArray());
                    txt.ToString();
                }));

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            downloadTask.StartDownload();
            stopwatch.Stop();

            stopwatch.Elapsed.ToString();
        }
    }
}
