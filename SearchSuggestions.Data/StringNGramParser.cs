namespace SearchSuggestions.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class StringNGramParser
    {
        public static async Task<string[]> GetNGrams(string s, int minLength, int maxLength)
        {
            if (minLength <= 1 || minLength > maxLength)
                throw new ArgumentOutOfRangeException(nameof(minLength));

            if (maxLength > s.Length)
                throw new ArgumentOutOfRangeException(nameof(maxLength));

            var nGrams = new List<string>();

            for (var i = minLength; i <= maxLength; i++)
            {
                nGrams.AddRange(await GetNGrams(s, i));
            }

            return nGrams.ToArray();
        }

        public static async Task<string> GetLongestNGramMatch(string x, string y)
        {
            var maxLength = Math.Min(x.Length, y.Length);
            for (var i = maxLength; i > 0; i--)
            {
                var nGrams1 = await GetNGrams(x, i);
                var nGrams2 = await GetNGrams(y, i);

                var intersection = nGrams1.Intersect(nGrams2);
                if (intersection.Any())
                    return intersection.First();
            }

            return null;
        }

        private static async Task<IEnumerable<string>> GetNGrams(string s, int length)
        {
            var nGrams = new List<string>();
            for (var i = 0; i <= s.Length - length; i++)
            {
                await Task.Run(() =>
                {
                    var subString = s.Substring(i, length);
                    nGrams.Add(subString);
                });
            }

            return nGrams;
        }
    }
}