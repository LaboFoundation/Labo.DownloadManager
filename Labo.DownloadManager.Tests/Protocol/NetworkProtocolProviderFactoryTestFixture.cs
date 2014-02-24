using Labo.DownloadManager.Protocol;
using Labo.DownloadManager.Protocol.Exceptions;
using Moq;
using NUnit.Framework;

namespace Labo.DownloadManager.Tests.Protocol
{
    using System;

    [TestFixture]
    public class NetworkProtocolProviderFactoryTestFixture
    {
        [TestCase("ftp://localhost/file.zip", "ftp", "ftp")]
        [TestCase("http://localhost/file.zip", "http", "http")]
        [TestCase("custom://localhost/file.zip", "custom", "http", ExpectedException = typeof(NetworkProtocolProviderFactoryException))]
        [Test]
        public void CreateProvider(string url, string protocol, string registerProtocol)
        {
            Mock<IUrlProtocolParser> urlProtocolParserMock = new Mock<IUrlProtocolParser>();
            urlProtocolParserMock.Setup(x => x.Parse(new Uri(url))).Returns(protocol);

            NetworkProtocolProviderFactory networkProtocolProviderFactory = new NetworkProtocolProviderFactory(urlProtocolParserMock.Object);
            
            INetworkProtocolProvider networkProtocolProvider = new Mock<INetworkProtocolProvider>().Object;
            networkProtocolProviderFactory.RegisterProvider(registerProtocol, networkProtocolProvider);

            Assert.AreEqual(networkProtocolProvider, networkProtocolProviderFactory.CreateProvider(new Uri(url)));
        }
    }
}
