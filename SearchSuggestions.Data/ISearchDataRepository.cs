namespace SearchSuggestions.Data
{
    using System.Collections.Generic;
    using Types;

    public interface ISearchDataRepository
    {
        IEnumerable<City> Get(IEnumerable<long> ids);
        City Get(long id);
    }
}