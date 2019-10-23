namespace SearchSuggestions.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Types;

    /// <summary>
    /// Mock class containing test city data
    /// </summary>
    internal class CityRepositoryMock : ISearchDataRepository
    {
        /// <summary>
        /// The cities
        /// </summary>
        private static Dictionary<long, City> cities;

        /// <summary>
        /// Initializes the test cities
        /// </summary>
        /// <param name="cityList">The list of cities</param>
        public static void InitializeCities(List<City> cityList)
        {
            cities = cityList.ToDictionary(c => c.Id, c => c);
        }

        /// <inheritdoc cref="ISearchDataRepository"/>
        public IEnumerable<City> Get(IEnumerable<long> ids)
        {
            return ids.Select(this.Get);
        }

        /// <inheritdoc cref="ISearchDataRepository"/>
        public City Get(long id)
        {
            return cities[id];
        }
    }
}