namespace SearchSuggestions.SearchEngine
{
    using System.Collections.Generic;
    using System.Linq;
    using Types;

    public class SearchResult
    {
        public SearchResult()
        {
            this.Results = new CitySuggestionResults();
        }

        public List<string> Errors { get; set; }

        public CitySuggestionResults Results { get; set; }

        public bool IsSuccess => this.Errors == null || !this.Errors.Any();
    }
}