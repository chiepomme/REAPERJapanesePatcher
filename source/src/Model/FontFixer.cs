using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAPERJapanesePatcher
{
    public class FontFixer
    {
        readonly byte[] DefaultFontSize = BitConverter.GetBytes((Int16)8);
        readonly byte[] Padding = new byte[] { 0x00, 0x00, 0x00, 0x01 };
        readonly byte[][] FontNames = new byte[][]{
            Encoding.Unicode.GetBytes("MS Sans Serif\0"),
            Encoding.Unicode.GetBytes("MS Shell Dlg\0"),
        };

        readonly byte[] RoutingDefaultWidth = BitConverter.GetBytes((Int16)205);
        readonly byte[] RoutingDefaultHeight = BitConverter.GetBytes((Int16)57);
        readonly byte[] RoutingNewHeight = BitConverter.GetBytes((Int16)80);
        readonly byte[] RoutingPadding = BitConverter.GetBytes((Int16)0);

        readonly byte[] ReaperDialogClassName = Encoding.Unicode.GetBytes("REAPERVirtWndDlgHost");

        public async Task FixFontSizeTo(string filePath, Byte fontSize)
        {
            var subsequences = FontNames.SelectMany(f => new Subsequence[]{
                new Subsequence(DefaultFontSize.Concat(f).ToArray()),
                new Subsequence(DefaultFontSize.Concat(Padding).Concat(f).ToArray()),
            });

            var bytes = await ReadToEndAsync(filePath);

            await Task.Run(() =>
            {
                foreach (var font in subsequences)
                {
                    var offset = 0;
                    while (true)
                    {
                        var foundPos = font.FindIn(bytes, offset);
                        if (!foundPos.HasValue) break;

                        var pos = foundPos.Value;
                        bytes[pos] = fontSize;

                        offset = pos + font.Length;
                    }
                }
            });

            // TODO: こっそりルーティングが隠れる問題を修正・・あとで切り出そう・・
            await Task.Run(() =>
            {
                var routingDialog = new Subsequence(RoutingDefaultWidth
                                            .Concat(RoutingDefaultHeight)
                                            .Concat(RoutingPadding)
                                            .Concat(ReaperDialogClassName)
                                            .ToArray()
                                        );
                var foundPos = routingDialog.FindIn(bytes, 0);
                if (!foundPos.HasValue) return;

                Buffer.BlockCopy(RoutingNewHeight.ToArray(), 0, bytes,
                    foundPos.Value + RoutingDefaultWidth.Length, RoutingNewHeight.Length);
            });

            using (var writer = new FileStream(filePath, FileMode.Open))
            {
                await writer.WriteAsync(bytes, 0, bytes.Length);
            }
        }

        async Task<byte[]> ReadToEndAsync(string filePath)
        {
            var fileSize = new FileInfo(filePath).Length;
            var bytes = new byte[fileSize];

            using (var reader = new FileStream(filePath, FileMode.Open))
            {
                await reader.ReadAsync(bytes, 0, bytes.Length);
            }

            return bytes;
        }
    }
}
