using System.IO;
using System.Threading.Tasks;

namespace REAPERJapanesePatcher
{
    public class AsyncFile
    {
        public static async Task<byte[]> ReadAllBytes(string filePath)
        {
            var fileSize = new FileInfo(filePath).Length;
            var bytes = new byte[fileSize];

            using (var reader = new FileStream(filePath, FileMode.Open))
            {
                await reader.ReadAsync(bytes, 0, bytes.Length);
            }

            return bytes;
        }

        public static async Task WriteAllBytes(string filePath, byte[] bytes)
        {
            using (var writer = new FileStream(filePath, FileMode.Open))
            {
                await writer.WriteAsync(bytes, 0, bytes.Length);
            }
        }
    }
}
