namespace SearchSuggestions.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Caching.Memory;

    /// <summary>
    /// Implementation of indexed data retrieval
    /// <see cref="IIndexedSearchDataRepository" />
    /// </summary>
    public class IndexedSearchDataRepository : IIndexedSearchDataRepository
    {
        /// <summary>
        /// The memory cache
        /// </summary>
        private readonly IMemoryCache dataCache;

        /// <summary>
        /// The indexed cache (to avoid retrieving from memory cache repeatedly)
        /// </summary>
        private Dictionary<string, List<long>> _indexedCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexedSearchDataRepository" /> class
        /// </summary>
        /// <param name="cache">The memory cache instance</param>
        public IndexedSearchDataRepository(IMemoryCache cache)
        {
            this.dataCache = cache;
        }

        /// <summary>
        /// Gets the indexed cache
        /// </summary>
        private Dictionary<string, List<long>> IndexedCache
        {
            get
            {
                if (this._indexedCache != null)
                    return this._indexedCache;

                var cacheValue = this.dataCache.Get<Dictionary<string, List<long>>>(Constants.IndexCacheKey);
                if (cacheValue == null)
                    throw new KeyNotFoundException(
                        $"The city cache {Constants.IndexCacheKey} was not properly loaded!");

                return this._indexedCache = cacheValue;
            }
        }

        /// <inheritdoc cref="IIndexedSearchDataRepository"/>
        public List<long> GetMatchingCities(string[] queryNgrams)
        {
            return queryNgrams.SelectMany(n => this.IndexedCache.TryGetValue(n, out var ids) ? ids : new List<long>())
                .Distinct()
                .ToList();
        }
    }
}