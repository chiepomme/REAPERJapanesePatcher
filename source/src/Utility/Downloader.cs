using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace REAPERJapanesePatcher
{
    public class Downloader
    {
        public async Task Download(Uri uri, string to, IProgress<DownloadProgress> progress = null)
        {
            var req = WebRequest.CreateHttp(uri);
            var res = await req.GetResponseAsync();

            using (var writer = new FileStream(to, FileMode.Create))
            {
                using (var reader = res.GetResponseStream())
                {
                    var buffer = new byte[0x100000];

                    long downloadedLength = 0;
                    int numBytesRead;
                    while ((numBytesRead = await reader.ReadAsync(buffer, 0, buffer.Length)) != 0)
                    {
                        downloadedLength += numBytesRead;
                        if (progress != null)
                            progress.Report(new DownloadProgress(downloadedLength, res.ContentLength));
                        await writer.WriteAsync(buffer, 0, numBytesRead);
                    }
                }
            }
        }

    }

    public class DownloadProgress
    {
        public readonly long DownloadedLength;
        public readonly long TotalLength;

        public DownloadProgress(long downloadedLength, long totalLength)
        {
            DownloadedLength = downloadedLength;
            TotalLength = totalLength;
        }
    }
}
