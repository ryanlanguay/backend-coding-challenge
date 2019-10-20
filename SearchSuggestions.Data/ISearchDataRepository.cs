namespace SearchSuggestions.Data
{
    using System.Collections.Generic;
    using Types;

    /// <summary>
    /// Interface for city data access
    /// </summary>
    public interface ISearchDataRepository
    {
        /// <summary>
        /// Gets a set of cities by their ID
        /// </summary>
        /// <param name="ids">The ids</param>
        /// <returns>The cities</returns>
        IEnumerable<City> Get(IEnumerable<long> ids);

        /// <summary>
        /// Gets a city by ID 
        /// </summary>
        /// <param name="id">The ID</param>
        /// <returns>The city</returns>
        City Get(long id);
    }
}