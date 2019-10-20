namespace SearchSuggestions.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Caching.Memory;
    using Types;

    /// <summary>
    /// Implementation of city data access
    /// <see cref="ISearchDataRepository" />
    /// </summary>
    public class SearchDataRepository : ISearchDataRepository
    {
        /// <summary>
        /// The memory cache
        /// </summary>
        private readonly IMemoryCache dataCache;

        /// <summary>
        /// The city cache (to avoid retrieving from memory cache repeatedly)
        /// </summary>
        private Dictionary<long, City> _cityCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchDataRepository" /> class
        /// </summary>
        /// <param name="cache">The cache</param>
        public SearchDataRepository(IMemoryCache cache)
        {
            this.dataCache = cache;
        }

        /// <summary>
        /// Gets the city cache
        /// </summary>
        private Dictionary<long, City> CityCache
        {
            get
            {
                if (this._cityCache != null)
                    return this._cityCache;

                var cacheValue = this.dataCache.Get<Dictionary<long, City>>(Constants.CityCacheKey);
                if (cacheValue == null)
                    throw new KeyNotFoundException($"The city cache {Constants.CityCacheKey} was not properly loaded!");

                return this._cityCache = cacheValue;
            }
        }

        /// <inheritdoc cref="ISearchDataRepository"/>
        public IEnumerable<City> Get(IEnumerable<long> ids)
        {
            return ids.Select(this.Get);
        }

        /// <inheritdoc cref="ISearchDataRepository"/>
        public City Get(long id)
        {
            return this.CityCache.TryGetValue(id, out var city) ? city : null;
        }
    }
}