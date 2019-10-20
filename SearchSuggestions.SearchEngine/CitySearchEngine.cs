namespace SearchSuggestions.SearchEngine
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Types;

    public class CitySearchEngine
    {
        private readonly IIndexedSearchDataRepository indexedSearchDataRepository;
        private readonly ISearchDataRepository searchDataRepository;
        private readonly IScorer<string> queryScorer;
        private readonly IScorer<LocationInformation> locationScorer;

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

        public async Task<SearchResult> Search(string query, LocationInformation locationInformation)
        {
            if (!this.ValidateSearchRequest(query, locationInformation, out var errors))
            {
                return new SearchResult
                {
                    Errors = errors,
                    Results = null
                };
            }
            
            var minLength = query.Length - 1;
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
                .ToList();

            return new SearchResult
            {
                Results = new CitySuggestionResults() {Suggestions = suggestions}
            };
        }

        private bool ValidateSearchRequest(
            string query, 
            LocationInformation locationInformation,
            out List<string> errors)
        {
            errors = new List<string>();

            if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
            {
                errors.Add("A query parameter of at least 2 is required.");
                return false;
            }

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
                if (MathHelper.IsInRange(-90, locationInformation.Latitude.Value, 90)
                    || MathHelper.IsInRange(-180, locationInformation.Longitude.Value, 180))
                {
                    errors.Add("Invalid latitude or longitude value. Latitudes are between -90 and 90, longitudes are between -180 and 180.");
                    return false;
                }
            }

            return true;
        }
    }
}