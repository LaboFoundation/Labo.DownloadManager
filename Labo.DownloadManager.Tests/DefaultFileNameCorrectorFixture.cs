namespace Labo.DownloadManager.Tests
{
    using System;
    using System.IO;

    using Labo.Common.Utils;

    using NUnit.Framework;

    [TestFixture]
    public class DefaultFileNameCorrectorFixture
    {
        [SetUp]
        public void SetUp()
        {
            string baseDirectory = GetBaseDirectory();

            IOUtils.DeleteDirectory(baseDirectory, true);
        }

        [Test]
        public void GetFileNameShouldReturnSpecifiedFileNameWhenNoFileExists()
        {
            string baseDirectory = GetBaseDirectory();

            string fileName = Path.Combine(baseDirectory, "a.txt");
            string expectedFileName = Path.Combine(baseDirectory, "a.txt");

            DefaultFileNameCorrector fileNameCorrector = new DefaultFileNameCorrector();
            Assert.AreEqual(expectedFileName, fileNameCorrector.GetFileName(fileName));
        }

        [Test]
        public void GetFileNameShouldReturnNextFileNameWhenTheFileExists()
        {
            string baseDirectory = GetBaseDirectory();
                        
            string fileName = Path.Combine(baseDirectory, "a.txt");
            string expectedFileName = Path.Combine(baseDirectory, "a(1).txt");

            File.WriteAllText(fileName, string.Empty);

            DefaultFileNameCorrector fileNameCorrector = new DefaultFileNameCorrector();
            Assert.AreEqual(expectedFileName, fileNameCorrector.GetFileName(fileName));
        }

        [Test]
        public void GetFileNameShouldReturnNextFileNameWhenTwoFilesExists()
        {
            string baseDirectory = GetBaseDirectory();

            string fileName = Path.Combine(baseDirectory, "a.txt");
            string expectedFileName = Path.Combine(baseDirectory, "a(2).txt");

            File.WriteAllText(Path.Combine(baseDirectory, "a.txt"), string.Empty);
            File.WriteAllText(Path.Combine(baseDirectory, "a(1).txt"), string.Empty);

            DefaultFileNameCorrector fileNameCorrector = new DefaultFileNameCorrector();
            Assert.AreEqual(expectedFileName, fileNameCorrector.GetFileName(fileName));
        }

        [Test]
        public void GetFileName()
        {
            string baseDirectory = GetBaseDirectory();

            string fileName = Path.Combine(baseDirectory, "a(1).txt");
            string expectedFileName = Path.Combine(baseDirectory, "a(1)(1).txt");

            File.WriteAllText(Path.Combine(baseDirectory, "a(1).txt"), string.Empty);

            DefaultFileNameCorrector fileNameCorrector = new DefaultFileNameCorrector();
            Assert.AreEqual(expectedFileName, fileNameCorrector.GetFileName(fileName));
        }

        private static string GetBaseDirectory()
        {
            string baseDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DefaultFileNameCorrector");

            IOUtils.EnsureDirectoryExists(baseDirectory);

            return baseDirectory;
        }
    }
}
