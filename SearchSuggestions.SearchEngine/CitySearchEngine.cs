namespace SearchSuggestions.SearchEngine
{
    using System;
    using System.Threading.Tasks;
    using Data;
    using Types;

    public class CitySearchEngine
    {
        private readonly IIndexedSearchDataRepository indexedSearchDataRepository;
        private readonly ISearchDataRepository searchDataRepository;

        public CitySearchEngine(
            ISearchDataRepository searchDataRepository,
            IIndexedSearchDataRepository indexedSearchDataRepository)
        {
            this.searchDataRepository = searchDataRepository;
            this.indexedSearchDataRepository = indexedSearchDataRepository;
        }

        public async Task<CitySuggestionResults> Search(string query, LocationInformation locationInformation)
        {
            throw new NotImplementedException();
        }
    }
}