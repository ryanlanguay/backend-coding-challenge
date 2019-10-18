namespace SearchSuggestions.SearchEngine
{
    using System;
    using System.Threading.Tasks;
    using Data;

    public class CityNameMatchScorer : IScorer<string>
    {
        public async Task<double> GetMatchScore(string expected, string actual)
        {
            var longestMatch = await StringNGramParser.GetLongestNGramMatch(expected.ToLower(), actual.ToLower());
            var matchLengthFactor = Convert.ToDouble(longestMatch.Length) / Convert.ToDouble(actual.Length);

            var startingFactor = actual.ToLower().StartsWith(longestMatch.ToLower()) ? 0.1d : 0;

            return Math.Min(matchLengthFactor + startingFactor, 1.0d);
        }
    }
}