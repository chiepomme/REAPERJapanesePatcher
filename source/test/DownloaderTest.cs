using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace REAPERJapanesePatcher.Test
{
    [TestClass]
    public class DownloaderTest
    {
        readonly Uri DownloadUri;
        readonly string DownloadDestinationPath;

        public DownloaderTest()
        {
            DownloadUri = JapanesePatcher.LangPackUriMap[JapanesePatcher.LangPackType.Master];
            DownloadDestinationPath = Path.Combine(Path.GetTempPath(), Path.GetFileName(DownloadUri.LocalPath));
        }

        [TestInitialize]
        public void Initialize()
        {
            if (File.Exists(DownloadDestinationPath)) File.Delete(DownloadDestinationPath);
        }

        [TestMethod]
        public async Task ファイルをダウンロードできる()
        {
            var downloader = new Downloader();
            await downloader.Download(DownloadUri, DownloadDestinationPath);
            Assert.IsTrue(File.Exists(DownloadDestinationPath));
            Assert.IsTrue(new FileInfo(DownloadDestinationPath).Length > 100000);
        }

        [TestMethod]
        public async Task ダウンロードの進行状況を取得できる()
        {
            var downloader = new Downloader();

            long? downloadedLength = null;
            long? totalLength = null;

            var progress = new Progress<DownloadProgress>((p) =>
                {
                    downloadedLength = p.DownloadedLength;
                    totalLength = p.TotalLength;
                }
            );

            await downloader.Download(DownloadUri, DownloadDestinationPath, progress);

            Assert.IsNotNull(downloadedLength);
            Assert.IsNotNull(totalLength);
            Assert.IsTrue(downloadedLength >= totalLength);
        }
    }
}
