namespace SearchSuggestions.Data
{
    /// <summary>
    /// Keys for internal memory cache
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// The key for the cities cache
        /// </summary>
        public const string CityCacheKey = "Cities";

        /// <summary>
        /// The key for the indexed cities cache
        /// </summary>
        public const string IndexCacheKey = "IndexedCities";

        /// <summary>
        /// The data file path config key
        /// </summary>
        public const string FilePathKey = "DataFilePath";

        /// <summary>
        /// The state file path key
        /// </summary>
        public const string StateFilePathKey = "StateFilePath";
    }
}