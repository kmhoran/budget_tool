using System;
using System.Collections.Generic;

namespace Extentions.Text
{
    public static class ByteExtentions
    {
        public static byte[] Replace(this byte[] source, byte[] search, byte[] replacement)
        {
            // ReplaceBytes(src, search, repl);
            List<byte> result = new List<byte>();
            var srcLen = source.Length;
            var schLen = search.Length;

            for (int i = 0; i < srcLen; i++)
            {
                // potential match
                if (source[i] == search[0] && i + schLen <= srcLen)
                {
                    var isMatch = true;
                    for (var schIndex = 0; schIndex < schLen; schIndex++)
                    {
                        if (source[schIndex + i] != search[schIndex]) isMatch = false;
                    }
                    // not a match, 
                    if (!isMatch)
                    {
                        result.Add(source[i]);
                        continue;
                    }
                    result.AddRange(replacement);
                    i += schLen - 1;
                    continue;
                }
                result.Add(source[i]);
            }

            return result.ToArray();
        }
    }
}
