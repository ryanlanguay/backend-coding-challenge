namespace SearchSuggestions.Data
{
    using System.Linq;
    using Microsoft.Extensions.Caching.Memory;

    public static class DataInitializer
    {
        public static void InitializeData(IMemoryCache cache)
        {
            // First load the cities from the data files
            var fileLoader = new DataFileLoader();
            fileLoader.LoadData();

            // City cache: key is city ID, value is City object
            var cityCache = fileLoader.Cities.ToDictionary(x => x.Id, y => y);
            cache.Set(CacheKeys.CityCacheKey, cityCache);
        }
    }
}