using System;
using Labo.DownloadManager.Protocol;
using NUnit.Framework;

namespace Labo.DownloadManager.Tests.Protocol
{
    [TestFixture]
    public class UrlProtocolParserTestFixture
    {
        [TestCase("http://www.google.com", "http")]
        [TestCase("http://www.google.com:80", "http")]
        [TestCase("http://www.google.com:80/a.zip", "http")]
        [TestCase("https://www.google.com:80/a.zip", "https")]
        [TestCase("ftp://www.google.com:80/a.zip", "ftp")]
        [TestCase("custom://www.google.com:80/a.zip", "custom")]
        [TestCase("http://localhost:81/a.zip", "http")]
        [TestCase(null, null, ExpectedException = typeof(ArgumentNullException))]
        [Test]
        public void Parse(string url, string expectedProtocol)
        {
            IUrlProtocolParser urlProtocolParser = new UrlProtocolParser();
            Assert.AreEqual(expectedProtocol, urlProtocolParser.Parse(new Uri(url)));
        }
    }
}
