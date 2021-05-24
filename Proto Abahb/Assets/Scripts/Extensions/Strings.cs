using System.Collections.Generic;
using System.Linq;

namespace Extensions
{
    public static class Strings
    {
        public static int[] IndexsOf(this string source, params char[] chars)
        {
            List<int> indexes = new List<int>();
            for (int i = 0; i < source.Length; i++)
            {
                var current = source[i];
                if (chars.Any(c => c == current)) indexes.Add(i);
            }
            return indexes.ToArray();
        }
        public static string[] SplitAt(this string input, bool exclude, params char[] chars)
        {
            var indexes = input.IndexsOf(chars);
            if (exclude) return input.SplitAt(indexes).Select(i => i.TrimStart(chars)).ToArray();
            else return input.SplitAt(indexes);
        }
        public static string[] SplitAtFirst(this string input, bool exclude, char c)
        {
            var index = input.IndexOf(c);
            if (exclude) return input.SplitAt(index).Select(i => i.TrimStart(c)).ToArray();
            else return input.SplitAt(index);
        }
        public static string[] SplitAt(this string source, params int[] index)
        {
            index = index.Distinct().OrderBy(x => x).ToArray();
            var length = source.Length;
            if (index.Any(i => i + 1 == length)) index = index.Where(i => i != length).ToArray();
            string[] output = new string[index.Length + 1];
            int pos = 0;

            for (int i = 0; i < index.Length; pos = index[i++])
                output[i] = source.Substring(pos, index[i] - pos);

            output[index.Length] = source.Substring(pos);
            return output;
        }
    }
}