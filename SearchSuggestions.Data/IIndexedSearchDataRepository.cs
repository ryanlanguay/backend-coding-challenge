namespace SearchSuggestions.Data
{
    using System.Collections.Generic;

    /// <summary>
    /// Interface for indexed data access
    /// </summary>
    public interface IIndexedSearchDataRepository
    {
        /// <summary>
        /// Gets a list of matching cities for a set of n-grams
        /// </summary>
        /// <param name="queryNgrams">The n-grams</param>
        /// <returns>The list of unique city IDs</returns>
        List<long> GetMatchingCities(string[] queryNgrams);
    }
}