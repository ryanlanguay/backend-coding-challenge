namespace SearchSuggestions.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Helper class to parse a string into n-grams, and work with the results
    /// </summary>
    public static class StringNGramParser
    {
        /// <summary>
        /// Gets the n-grams for a specific string
        /// </summary>
        /// <param name="s">The string</param>
        /// <param name="minLength">The minimum n-gram length</param>
        /// <param name="maxLength">The maximum n-gram length</param>
        /// <returns>The n-grams of the string</returns>
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

        /// <summary>
        /// Gets the longest n-gram match between two string
        /// </summary>
        /// <param name="x">The first string</param>
        /// <param name="y">The second string</param>
        /// <returns>The longest match (or null if no match)</returns>
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

        /// <summary>
        /// Gets n-grams of a given length from a string
        /// </summary>
        /// <param name="s">The string</param>
        /// <param name="length">The length</param>
        /// <returns>The n-grams</returns>
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