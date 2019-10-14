namespace SearchSuggestions.Data
{
    using System.Collections.Generic;

    public interface IIndexedSearchDataRepository
    {
        List<long> GetMatchingCities(string[] queryNgrams);
    }
}