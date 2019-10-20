namespace SearchSuggestions.SearchEngine
{
    internal static class SearchScoringFactors
    {
        public const double LocationScoringFactor = 0.1d;

        public const double MaximumSearchScore = 0.9d;

        public const double SearchStartingScore = 0.1d;

        public const double LongestMatchBonus = 0.25d;
    }
}