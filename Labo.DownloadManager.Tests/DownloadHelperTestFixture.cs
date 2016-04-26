namespace Labo.DownloadManager.Tests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Threading;

    using Labo.DownloadManager.Settings;
    using Labo.DownloadManager.Streaming;

    using NUnit.Framework;

    [TestFixture]
    public class DownloadHelperTestFixture
    {
        private class TestDownloadStreamManager : IDownloadStreamManager
        {
            public Stream CreateStream(RemoteFileInfo remoteFileInfo)
            {
                return new TestOutputStream(File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\images", "087dc7cf14c97_1.jpg")));
            }
        }

        [Test]
        public void Test()
        {
            string imagesPath = Path.Combine(Environment.CurrentDirectory, "images");
            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }

            DownloadHelper downloadHelper = new DownloadHelper(new InMemoryDownloadSettings(200, 5, 1024, 5, 5, 0), new LocalFileDownloadStreamManager(new TempFileAllocator(new DefaultFileNameCorrector())));

            downloadHelper.AddNewDownloadTask(new Uri("http://dc455.4shared.com/img/OctFRjPl/s3/13470492140/marika-fruscio-01.jpg"), Path.Combine(imagesPath, "marika-fruscio-01.jpg"), 5);

            downloadHelper.OnDownloadFinished(
               x =>
               {
                   x.DownloadTask.State.ToString();
               });

            downloadHelper.Start();
            downloadHelper.Shutdown(true);
        }

        [Test]
        public void Manual()
        {
            string imagesPath = Path.Combine(Environment.CurrentDirectory, "images");
            if (!Directory.Exists(imagesPath))
            {
                Directory.CreateDirectory(imagesPath);
            }

            MemoryStream memoryStream = new MemoryStream();
            List<Thread> threads = new List<Thread>();
            threads.Add(GetThread(0, 31442, memoryStream));
            threads.Add(GetThread(31443, 38364, memoryStream));
            //threads.Add(GetThread(62886, 94328, memoryStream));
            //threads.Add(GetThread(94329, 125771, memoryStream));
            //threads.Add(GetThread(125772, 157214, memoryStream));
            //threads.Add(GetThread(157215, 157220, memoryStream));

            foreach (Thread thread in threads)
            {
                thread.Start();

                // Thread.Sleep(1);
            }

            foreach (Thread thread in threads)
            {
                thread.Join();
            }

            var array = memoryStream.ToArray();

            //var original = File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\images", "marika-fruscio-01.jpg"));
            //for (int i = 0; i < original.Length; i++)
            //{
            //    Assert.IsTrue(original[i] == array[i]);
            //}

            File.WriteAllBytes(Path.Combine(imagesPath, "marika-fruscio-01.jpg"), array);
        }

        private static Thread GetThread(int @from, int to, MemoryStream memoryStream)
        {
           return new Thread(
                () =>
                    {
                        const int bufferSize = 1024;

                        byte[] buffer = new byte[bufferSize];

                        WebResponse webResponse;
                        Stream stream;
                        lock (typeof(HttpWebRequest))
                        {
                            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri("http://dc455.4shared.com/img/OctFRjPl/s3/13470492140/marika-fruscio-01.jpg"));
                            webRequest.AddRange(@from, to);

                            webResponse = webRequest.GetResponse();
                            stream = webResponse.GetResponseStream();
                        }
                       

                        long length = long.Parse(webResponse.Headers[HttpResponseHeader.ContentLength]);
                        length.ToString();

                        int bytesRead;
                        int position = @from;
                        while ((bytesRead = stream.Read(buffer, 0, bufferSize)) != 0)
                        {
                            memoryStream.Position = position; 

                            memoryStream.Write(buffer, 0, bytesRead);

                            position += bytesRead;
                        }
                    });
        }
    }
}
