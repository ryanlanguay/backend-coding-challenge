namespace SearchSuggestions.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Configuration;
    using Serilog;

    /// <summary>
    /// Initializes the application data cache
    /// </summary>
    public static class DataInitializer
    {
        /// <summary>
        /// Initializes data in the memory cache
        /// </summary>
        /// <param name="config">The configuration</param>
        /// <param name="cache">The memory cache instance</param>
        public static async void InitializeData(IConfiguration config, IMemoryCache cache)
        {
            try
            {
                Log.Logger.Information("Data loading begin.");

                // First load the cities from the data files
                var fileLoader = new DataFileLoader(config);
                var cities = fileLoader.LoadData();

                // City cache: key is city ID, value is City object
                var cityCache = cities.ToDictionary(x => x.Id, y => y);
                cache.Set(Constants.CityCacheKey, cityCache);

                // Indexed cache: key is an n-gram, value is the city Id
                var indexedCache = new Dictionary<string, List<long>>(StringComparer.InvariantCultureIgnoreCase);
                foreach (var city in cities)
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
                            indexedCache[ngram] = new List<long> {city.Id};
                        }
                    }
                }

                cache.Set(Constants.IndexCacheKey, indexedCache);
            }
            catch (Exception e)
            {
                Log.Logger.Fatal(e, "An error occurred loading the cache.");
            }
            finally
            {
                Log.Logger.Information("Data loading end.");
            }
        }
    }
}