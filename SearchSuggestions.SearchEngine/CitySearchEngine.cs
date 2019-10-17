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

        public async Task<CitySuggestionResults> Search(string query, LocationInformation locationInformation)
        {
            var minLength = Math.Max(2, query.Length - 2);
            var queryNgrams = await StringNGramParser.GetNGrams(query, minLength, query.Length);
            var matchingCities = this.indexedSearchDataRepository.GetMatchingCities(queryNgrams);

            if (!matchingCities.Any())
                return new CitySuggestionResults();

            // We have some matches; get the scores for them
            var cities = this.searchDataRepository.Get(matchingCities);
            var suggestions = new List<CitySuggestionResult>();

            foreach (var city in cities)
            {
                var searchScore = await this.queryScorer.GetMatchScore(query, city.Name);
                var locationScore = await this.locationScorer.GetMatchScore(locationInformation, city.LocationInformation);
                var totalScore = searchScore;

                if (locationInformation.Latitude.HasValue && locationInformation.Longitude.HasValue)
                {
                    totalScore = (0.75 * searchScore + 0.25 * locationScore) / (searchScore + locationScore);
                }
                else if (locationInformation.Latitude.HasValue || locationInformation.Longitude.HasValue)
                {
                    totalScore = (0.5 * searchScore + 0.5 * locationScore) / (searchScore + locationScore);
                }

                suggestions.Add(new CitySuggestionResult
                {
                    Latitude = city.LocationInformation.Latitude.GetValueOrDefault(),
                    Longitude = city.LocationInformation.Longitude.GetValueOrDefault(),
                    Name = city.DisplayName,
                    Score = totalScore
                });
            }

            suggestions = suggestions.OrderByDescending(s => s.Score).ToList();
            return new CitySuggestionResults { Suggestions = suggestions };
        }
    }
}