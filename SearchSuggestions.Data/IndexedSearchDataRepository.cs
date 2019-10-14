namespace SearchSuggestions.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Caching.Memory;

    public class IndexedSearchDataRepository : IIndexedSearchDataRepository
    {
        private readonly IMemoryCache dataCache;
        private Dictionary<string, List<long>> _indexedCache;

        public IndexedSearchDataRepository(IMemoryCache cache)
        {
            this.dataCache = cache;
        }

        private Dictionary<string, List<long>> IndexedCache
        {
            get
            {
                if (this._indexedCache != null)
                    return this._indexedCache;

                var cacheValue = this.dataCache.Get<Dictionary<string, List<long>>>(CacheKeys.IndexCacheKey);
                if (cacheValue == null)
                    throw new KeyNotFoundException(
                        $"The city cache {CacheKeys.IndexCacheKey} was not properly loaded!");

                return this._indexedCache = cacheValue;
            }
        }

        public List<long> GetMatchingCities(string[] queryNgrams)
        {
            return queryNgrams.SelectMany(n => this.IndexedCache.TryGetValue(n, out var ids) ? ids : new List<long>())
                .Distinct()
                .ToList();
        }
    }
}