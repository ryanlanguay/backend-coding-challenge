namespace SearchSuggestions.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Types;

    internal class CityRepositoryMock : ISearchDataRepository
    {
        private static Dictionary<long, City> cities;

        public IEnumerable<City> Get(IEnumerable<long> ids)
        {
            return ids.Select(this.Get);
        }

        public City Get(long id)
        {
            return cities[id];
        }

        public static void InitializeCities(List<City> cityList)
        {
            cities = cityList.ToDictionary(c => c.Id, c => c);
        }
    }
}