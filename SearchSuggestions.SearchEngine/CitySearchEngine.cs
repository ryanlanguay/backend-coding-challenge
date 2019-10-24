namespace SearchSuggestions.SearchEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Types;

    /// <summary>
    /// Main search engine class
    /// Used to search for cities by name and location
    /// </summary>
    public class CitySearchEngine
    {
        /// <summary>
        /// The indexed data repository
        /// </summary>
        private readonly IIndexedSearchDataRepository indexedSearchDataRepository;

        /// <summary>
        /// The main search data repository (un-indexed)
        /// </summary>
        private readonly ISearchDataRepository searchDataRepository;

        /// <summary>
        /// The query (name) scorer
        /// </summary>
        private readonly IScorer<string> queryScorer;

        /// <summary>
        /// The location scorer
        /// </summary>
        private readonly IScorer<LocationInformation> locationScorer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CitySearchEngine" /> class
        /// </summary>
        /// <param name="searchDataRepository">The search data repository</param>
        /// <param name="indexedSearchDataRepository">The indexed data repository</param>
        /// <param name="queryScorer">The query scorer</param>
        /// <param name="locationScorer">The location scorer</param>
        public CitySearchEngine(
            ISearchDataRepository searchDataRepository,
            IIndexedSearchDataRepository indexedSearchDataRepository,
            IScorer<string> queryScorer,
            IScorer<LocationInformation> locationScorer)
        {
            this.searchDataRepository = searchDataRepository;
            this.indexedSearchDataRepository = indexedSearchDataRepository;
            this.queryScorer = queryScorer;
            this.locationScorer = locationScorer;
        }

        /// <summary>
        /// Searches for matching cities
        /// </summary>
        /// <param name="query">The query string. Can be a complete or partial city name</param>
        /// <param name="locationInformation">The searcher's location information</param>
        /// <returns>The results of the search</returns>
        public async Task<SearchResult> Search(string query, LocationInformation locationInformation)
        {
            // Validate the request; if it's invalid, we stop here
            if (!ValidateSearchRequest(query, locationInformation, out var errors))
            {
                return new SearchResult
                {
                    Errors = errors,
                    Results = null
                };
            }
            
            // We use the most of the provided search criteria to get more relevant results
            // This means we cannot account for typos, but reduces the noise of the results we return
            var minLength = Math.Max(2, query.Length);
            var queryNgrams = await StringNGramParser.GetNGrams(query, minLength, query.Length);
            var matchingCities = this.indexedSearchDataRepository.GetMatchingCities(queryNgrams);

            if (!matchingCities.Any())
                return new SearchResult();

            // We have some matches; get the scores for them
            var cities = this.searchDataRepository.Get(matchingCities);
            var suggestions = new List<CitySuggestionResult>();

            foreach (var city in cities)
            {
                var searchScore = await this.queryScorer.GetMatchScore(query, city.Name);
                var locationScore = await this.locationScorer.GetMatchScore(locationInformation, city.LocationInformation);
                var totalScore = MathHelper.GetBoundedValue(0, searchScore + locationScore, 1);

                suggestions.Add(new CitySuggestionResult
                {
                    Latitude = city.LocationInformation.Latitude.GetValueOrDefault(),
                    Longitude = city.LocationInformation.Longitude.GetValueOrDefault(),
                    Name = city.DisplayName,
                    NameScore = searchScore,
                    LocationScore = locationScore,
                    Score = totalScore
                });
            }

            // When we return the results, we want to return them in descending order of relevance
            suggestions = suggestions
                .OrderByDescending(s => s.Score)
                .ThenByDescending(s => s.NameScore)
                .ThenByDescending(s => s.LocationScore)
                .ToList();

            return new SearchResult
            {
                Results = new CitySuggestionResults() {Suggestions = suggestions}
            };
        }

        private static bool ValidateSearchRequest(
            string query, 
            LocationInformation locationInformation,
            out List<string> errors)
        {
            errors = new List<string>();

            // We don't want to search if the user has only input one character
            // (the search won't yield meaningful results in that case)
            if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
            {
                errors.Add("A query parameter of at least 2 is required.");
                return false;
            }

            // Location information is optional
            if (locationInformation != null && (locationInformation.Latitude.HasValue || locationInformation.Longitude.HasValue))
            {
                // Only one of the two values is provided
                if (locationInformation.Latitude.HasValue && !locationInformation.Longitude.HasValue
                    || !locationInformation.Latitude.HasValue && locationInformation.Longitude.HasValue)
                {
                    errors.Add("Only one of latitude or longitude provided. Either both or neither must be provided.");
                    return false;
                }

                // The values provided are not in the valid range
                if (!MathHelper.IsInRange(-90, locationInformation.Latitude.Value, 90)
                    || !MathHelper.IsInRange(-180, locationInformation.Longitude.Value, 180))
                {
                    errors.Add("Invalid latitude or longitude value. Latitudes are between -90 and 90, longitudes are between -180 and 180.");
                    return false;
                }
            }

            return true;
        }
    }
}