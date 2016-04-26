using System;
using System.Net;
using Labo.DownloadManager.Protocol;
using Moq;
using NUnit.Framework;

namespace Labo.DownloadManager.Tests.Protocol
{
    [TestFixture]
    public class WebRequestManagerTestFixture
    {
        [TestCase("http://localhost/file.zip", true, "user", "password")]
        [TestCase("http://localhost/file.zip", false, "", "")]
        [Test]
        public void GetWebRequest(string url, bool authenticate, string userName, string password)
        {
            int timeout = 0;
            ICredentials networkCredential = null;

            Mock<IWebRequestFactory> webRequestFactoryMock = new Mock<IWebRequestFactory>();
            Mock<WebRequest> webRequestMock = new Mock<WebRequest>();
            webRequestMock.SetupSet(x => x.Timeout).Callback(x => timeout = x);
            webRequestMock.SetupSet(x => x.Credentials).Callback(x => networkCredential = x);
            webRequestFactoryMock.Setup(x => x.CreateRequest(new Uri(url))).Returns(() => webRequestMock.Object);

            WebRequestManager webRequestManager = new WebRequestManager(webRequestFactoryMock.Object);
            WebRequest webRequest = webRequestManager.GetWebRequest(authenticate ? new DownloadFileRequestInfo(new Uri(url), userName, password) : new DownloadFileRequestInfo(new Uri(url)));

            Assert.AreSame(webRequestMock.Object, webRequest);
            Assert.AreEqual(30000, timeout);
            if (authenticate)
            {
                Assert.AreEqual(((NetworkCredential)networkCredential).UserName, userName);
                Assert.AreEqual(((NetworkCredential)networkCredential).Password, password);
            }
        }
    }
}
