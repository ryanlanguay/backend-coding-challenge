namespace SearchSuggestions.SearchEngine
{
    using System;
    using System.Threading.Tasks;
    using Data;

    public class CityNameMatchScorer : IScorer<string>
    {
        public async Task<double> GetMatchScore(string expected, string actual)
        {
            var longestMatch = await StringNGramParser.GetLongestNGramMatch(expected, actual);
            return longestMatch / Convert.ToDouble(actual.Length);
        }
    }
}