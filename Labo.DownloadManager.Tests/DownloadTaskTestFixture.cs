using System.Diagnostics;
using System.IO;
using System.Text;

using Labo.DownloadManager.EventAggregator;
using Labo.DownloadManager.EventArgs;
using Labo.DownloadManager.Protocol;
using Labo.DownloadManager.Protocol.Providers;
using Labo.DownloadManager.Segment;
using Labo.DownloadManager.Settings;
using Labo.DownloadManager.Streaming;

using NUnit.Framework;

namespace Labo.DownloadManager.Tests
{
    [TestFixture]
    public class DownloadTaskTestFixture
    {
        [Test, Ignore("written to see what is happening")]
        public void Download()
        {
            NetworkProtocolProviderFactory networkProtocolProviderFactory = new NetworkProtocolProviderFactory(new UrlProtocolParser());
            HttpProtocolProvider httpProtocolProvider = new HttpProtocolProvider(new WebRequestManager(new WebRequestFactory()));
            networkProtocolProviderFactory.RegisterProvider("http", httpProtocolProvider);
            networkProtocolProviderFactory.RegisterProvider("https", httpProtocolProvider);

            DownloadTask downloadTask = new DownloadTask(networkProtocolProviderFactory,
                                                          new DownloadSegmentPositionsCalculator(),
                                                          new MemoryDownloadStreamManager(),
                                                          new InMemoryDownloadSettings(200, 5, 8096, 5),
                                                          new DownloadFileInfo
                                                              {
                                                                  Url = "https://androidnetworktester.googlecode.com/files/1mb.txt",
                                                                  SegmentCount = 5
                                                              }, new EventManager());

            downloadTask.EventManager.EventSubscriber.RegisterConsumer(new ActionEventConsumer<DownloadTaskFinishedEventMessage>(x =>
                {
                    string txt = Encoding.UTF8.GetString(((MemoryStream)x.DownloadStream).ToArray());
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
