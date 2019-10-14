namespace SearchSuggestions.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Caching.Memory;
    using Types;

    public class SearchDataRepository
    {
        private readonly IMemoryCache dataCache;

        public SearchDataRepository(IMemoryCache cache)
        {
            this.dataCache = cache;
        }

        private Dictionary<long, City> CityCache => this.dataCache.Get<Dictionary<long, City>>(CacheKeys.CityCacheKey);

        public IEnumerable<City> Get(IEnumerable<long> ids)
        {
            return ids.Select(this.Get);
        }

        public City Get(long id)
        {
            return this.CityCache.TryGetValue(id, out var city) ? city : null;
        }
    }
}