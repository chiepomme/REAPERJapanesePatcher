using System;
using System.Collections.Generic;
using System.Linq;

namespace REAPERJapanesePatcher
{
    // http://www.geocities.jp/m_hiroi/light/pyalgo11.html
    public class Subsequence
    {
        public readonly int Length;
        readonly byte[] Subseq;
        readonly int[] MovementMap;

        public Subsequence(byte[] subseq)
        {
            Subseq = subseq;
            Length = subseq.Length;

            MovementMap = new int[sizeof(byte) * 256];

            for (var key = 0; key < MovementMap.Length; key++)
            {
                MovementMap[key] = Length + 1;
            }

            for (var key = 0; key < Length; key++)
            {
                MovementMap[Subseq[key]] = Length - key;
            }
        }

        public int? FindIn(byte[] source, int offset)
        {
            var sourceLength = source.Length;

            if (offset >= sourceLength) return null;

            var sourcePos = offset;
            while (sourcePos < sourceLength - Length + 1)
            {
                var subseqPos = 0;
                while (subseqPos < Length)
                {
                    if (source[sourcePos + subseqPos] != Subseq[subseqPos]) break;
                    subseqPos++;
                }

                if (subseqPos == Length)
                {
                    return sourcePos;
                }
                else
                {
                    if (sourcePos + Length == sourceLength)
                        return null;
                    else
                        sourcePos += MovementMap[source[sourcePos + Length]];
                }
            }

            return null;
        }
    }
}
