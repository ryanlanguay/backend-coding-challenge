namespace SearchSuggestions.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Caching.Memory;
    using Types;

    public class SearchDataRepository : ISearchDataRepository
    {
        private readonly IMemoryCache dataCache;
        private Dictionary<long, City> _cityCache;

        public SearchDataRepository(IMemoryCache cache)
        {
            this.dataCache = cache;
        }

        private Dictionary<long, City> CityCache
        {
            get
            {
                if (this._cityCache != null)
                    return this._cityCache;

                var cacheValue = this.dataCache.Get<Dictionary<long, City>>(CacheKeys.CityCacheKey);
                if (cacheValue == null)
                    throw new KeyNotFoundException($"The city cache {CacheKeys.CityCacheKey} was not properly loaded!");

                return this._cityCache = cacheValue;
            }
        }

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