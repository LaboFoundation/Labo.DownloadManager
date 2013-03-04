using System;
using Labo.DownloadManager.Protocol;
using NUnit.Framework;

namespace Labo.DownloadManager.Tests.Protocol
{
    [TestFixture]
    public class NetworkCredentialUserNameTestFixture
    {
        [TestCase("usr1", "", "usr1")]
        [TestCase("usr1\\domain1", "domain1", "usr1")]
        [TestCase("usr1/domain1", "", "usr1/domain1")]
        [TestCase("\\domain1", "domain1", "")]
        [TestCase(null, "", "", ExpectedException = typeof(ArgumentNullException))]
        [Test]
        public void Create(string userName, string expectedDomain, string expectedUserName)
        {
            NetworkCredentialUserName networkCredentialUserName = new NetworkCredentialUserName(userName);

            Assert.AreEqual(expectedDomain, networkCredentialUserName.Domain);
            Assert.AreEqual(expectedUserName, networkCredentialUserName.UserName);
        }
    }
}
