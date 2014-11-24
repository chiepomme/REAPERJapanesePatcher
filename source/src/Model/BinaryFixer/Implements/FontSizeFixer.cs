using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace REAPERJapanesePatcher
{
    public class FontSizeFixer : IBinaryFixer
    {
        static readonly byte[] DefaultFontSize = BitConverter.GetBytes((Int16)8);
        static readonly byte[] Padding = new byte[] { 0x00, 0x00, 0x00, 0x01 };
        static readonly byte[][] FontNames = new byte[][]{
            Encoding.Unicode.GetBytes("MS Sans Serif\0"),
            Encoding.Unicode.GetBytes("MS Shell Dlg\0"),
        };
        static readonly byte CorrectFontSize = 9;
        static readonly IEnumerable<SubsequenceFinder> FontNameSubsequences;

        static FontSizeFixer()
        {
            FontNameSubsequences = FontNames.SelectMany(fontName => new SubsequenceFinder[]{
                new SubsequenceFinder(DefaultFontSize.Concat(fontName).ToArray()),
                new SubsequenceFinder(DefaultFontSize.Concat(Padding).Concat(fontName).ToArray()),
            });
        }

        public void Fix(byte[] bytes)
        {
            foreach (var fontNameSequence in FontNameSubsequences)
            {
                var offset = 0;
                while (true)
                {
                    var foundPos = fontNameSequence.FindIn(bytes, offset);
                    if (!foundPos.HasValue) break;

                    var pos = foundPos.Value;
                    bytes[pos] = CorrectFontSize;

                    offset = pos + fontNameSequence.Length;
                }
            }
        }
    }
}
