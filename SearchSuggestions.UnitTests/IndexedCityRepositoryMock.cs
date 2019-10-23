namespace SearchSuggestions.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Data;
    using Types;

    /// <summary>
    /// Mock class for indexed city data
    /// </summary>
    internal class IndexedCityRepositoryMock : IIndexedSearchDataRepository
    {
        /// <summary>
        /// The cities
        /// </summary>
        private static Dictionary<string, List<long>> cities;

        /// <summary>
        /// Initializes the mock city database
        /// </summary>
        /// <param name="cityList">The list of cities (un-indexed)</param>
        public static void InitializeCities(List<City> cityList)
        {
            cities = new Dictionary<string, List<long>>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var city in cityList)
            {
                var maxLength = Math.Max(city.Name.Length, 2);
                var nGrams = StringNGramParser.GetNGrams(city.Name, 2, maxLength)
                    .ConfigureAwait(false)
                    .GetAwaiter()
                    .GetResult();

                foreach (var ngram in nGrams)
                {
                    if (cities.ContainsKey(ngram))
                    {
                        cities[ngram].Add(city.Id);
                    }
                    else
                    {
                        cities[ngram] = new List<long> { city.Id };
                    }
                }
            }
        }

        /// <inheritdoc cref="IIndexedSearchDataRepository"/>
        public List<long> GetMatchingCities(string[] queryNgrams)
        {
            return queryNgrams.SelectMany(n => cities.TryGetValue(n, out var ids) ? ids : new List<long>())
                .Distinct()
                .ToList();
        }
    }
}