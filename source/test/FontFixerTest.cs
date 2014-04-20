using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAPERJapanesePatcher.Test
{
    [TestClass]
    public class FontFixerTest
    {
        readonly string FixturePath = @"BinaryFixture.dat";
        readonly byte[] SansSerif = Encoding.Unicode.GetBytes("MS Sans Serif\0");
        readonly byte[] ShellDlg = Encoding.Unicode.GetBytes("MS Shell Dlg\0");
        readonly byte[] FontSize = BitConverter.GetBytes((Int16)8);
        readonly byte[] Padding = new byte[] { 0x00, 0x00, 0x00, 0x01 };

        readonly int RepeatTimes = 100;

        async Task CreateFixture()
        {
            using (var writer = new FileStream(FixturePath, FileMode.Create))
            {
                for (var i = 0; i < RepeatTimes; i++)
                {
                    await WriteAsync(writer, FontSize);
                    await WriteAsync(writer, SansSerif);

                    await WriteAsync(writer, FontSize);
                    await WriteAsync(writer, Padding);
                    await WriteAsync(writer, SansSerif);

                    await WriteAsync(writer, FontSize);
                    await WriteAsync(writer, ShellDlg);

                    await WriteAsync(writer, FontSize);
                    await WriteAsync(writer, Padding);
                    await WriteAsync(writer, ShellDlg);
                }
            }
        }

        async Task WriteAsync(FileStream writer, byte[] buffer)
        {
            await writer.WriteAsync(buffer, 0, buffer.Length);
        }

        async Task<byte[]> ReadAsync(FileStream reader, int length)
        {
            var buffer = new byte[length];
            await reader.ReadAsync(buffer, 0, length);
            return buffer;
        }

        [TestMethod]
        public async Task フォントサイズを修正出来る()
        {
            await CreateFixture();

            var fixer = new FontFixer();
            await fixer.FixFontSizeTo(FixturePath, 9);

            using (var reader = new FileStream(FixturePath, FileMode.Open))
            {
                var expectedFontSize = BitConverter.GetBytes((Int16)9);

                for (var i = 0; i < RepeatTimes; i++)
                {
                    Assert.IsTrue(expectedFontSize.SequenceEqual(await ReadAsync(reader, expectedFontSize.Length)));
                    Assert.IsTrue(SansSerif.SequenceEqual(await ReadAsync(reader, SansSerif.Length)));

                    Assert.IsTrue(expectedFontSize.SequenceEqual(await ReadAsync(reader, expectedFontSize.Length)));
                    Assert.IsTrue(Padding.SequenceEqual(await ReadAsync(reader, Padding.Length)));
                    Assert.IsTrue(SansSerif.SequenceEqual(await ReadAsync(reader, SansSerif.Length)));

                    Assert.IsTrue(expectedFontSize.SequenceEqual(await ReadAsync(reader, expectedFontSize.Length)));
                    Assert.IsTrue(ShellDlg.SequenceEqual(await ReadAsync(reader, ShellDlg.Length)));

                    Assert.IsTrue(expectedFontSize.SequenceEqual(await ReadAsync(reader, expectedFontSize.Length)));
                    Assert.IsTrue(Padding.SequenceEqual(await ReadAsync(reader, Padding.Length)));
                    Assert.IsTrue(ShellDlg.SequenceEqual(await ReadAsync(reader, ShellDlg.Length)));
                }
            }
        }
    }
}
