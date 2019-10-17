namespace SearchSuggestions.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Caching.Memory;

    public static class DataInitializer
    {
        public static async void InitializeData(IMemoryCache cache)
        {
            // First load the cities from the data files
            var fileLoader = new DataFileLoader();
            fileLoader.LoadData();

            // City cache: key is city ID, value is City object
            var cityCache = fileLoader.Cities.ToDictionary(x => x.Id, y => y);
            cache.Set(CacheKeys.CityCacheKey, cityCache);

            // Indexed cache: key is an n-gram, value is the city Id
            var indexedCache = new Dictionary<string, List<long>>(StringComparer.InvariantCultureIgnoreCase);
            foreach (var city in fileLoader.Cities)
            {
                var maxLength = Math.Max(city.Name.Length, 2);
                var nGrams = await StringNGramParser.GetNGrams(city.Name, 2, maxLength);
                foreach (var ngram in nGrams)
                {
                    if (indexedCache.ContainsKey(ngram))
                    {
                        indexedCache[ngram].Add(city.Id);
                    }
                    else
                    {
                        indexedCache[ngram] = new List<long> { city.Id };
                    }
                }
            }

            cache.Set(CacheKeys.IndexCacheKey, indexedCache);
        }
    }
}