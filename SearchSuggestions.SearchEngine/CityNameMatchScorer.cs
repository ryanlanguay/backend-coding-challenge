namespace SearchSuggestions.SearchEngine
{
    using System;
    using System.Threading.Tasks;
    using Data;

    /// <summary>
    /// Scoring class for city names
    /// <seealso cref="IScorer{T}"/>
    /// </summary>
    public class CityNameMatchScorer : IScorer<string>
    {
        /// <inheritdoc cref="IScorer{T}" />
        public async Task<double> GetMatchScore(string received, string actual)
        {
            // Early exit: if the search criteria matches the value exactly, return the maximum possible value
            if (string.Equals(received, actual, StringComparison.InvariantCultureIgnoreCase))
                return SearchScoringFactors.MaximumSearchScore;

            // Check the ratio of the longest n-gram to the length of the actual string
            var longestMatch = await StringNGramParser.GetLongestNGramMatch(received.ToLower(), actual.ToLower());
            var matchLengthFactor = Convert.ToDouble(longestMatch.Length) / Convert.ToDouble(actual.Length);

            // Bonus if the longest n-gram is actually the search criteria
            var longestMatchBonus = string.Equals(longestMatch, received, StringComparison.InvariantCultureIgnoreCase) 
                ? SearchScoringFactors.LongestMatchBonus 
                : 0d;

            // If the longest n-gram is at the beginning of the string, give a bonus
            var startingFactor = actual.ToLower().StartsWith(longestMatch.ToLower()) ? SearchScoringFactors.SearchStartingScore : 0d;

            return Math.Min(matchLengthFactor + longestMatchBonus + startingFactor, SearchScoringFactors.MaximumSearchScore);
        }
    }
}