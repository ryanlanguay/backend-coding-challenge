namespace SearchSuggestions.SearchEngine
{
    using System.Collections.Generic;
    using System.Linq;
    using Types;

    /// <summary>
    /// The results of a search request
    /// Includes error messaging and the suggestion results
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchResult" /> class
        /// </summary>
        public SearchResult()
        {
            this.Results = new CitySuggestionResults();
        }

        /// <summary>
        /// Gets or sets the list of errors for this request
        /// </summary>
        public List<string> Errors { get; set; }

        /// <summary>
        /// Gets or sets the results of the search
        /// </summary>
        public CitySuggestionResults Results { get; set; }

        /// <summary>
        /// Gets a value indicating if the current request ended successfully
        /// </summary>
        public bool IsSuccess => this.Errors == null || !this.Errors.Any();
    }
}