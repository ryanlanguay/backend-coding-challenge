namespace SearchSuggestions.SearchEngine
{
    /// <summary>
    /// Factors used in scoring
    /// </summary>
    internal static class SearchScoringFactors
    {
        /// <summary>
        /// The location scoring base factor
        /// </summary>
        public const double LocationScoringFactor = 0.1d;

        /// <summary>
        /// The maximum score that can be returned from a name match
        /// </summary>
        public const double MaximumSearchScore = 0.85d;

        /// <summary>
        /// The factor used when an n-gram matches the beginning of a string
        /// </summary>
        public const double SearchStartingScore = 0.1d;

        /// <summary>
        /// The factor used when the longest n-gram match is equal to the search criteria
        /// </summary>
        public const double LongestMatchBonus = 0.25d;
    }
}