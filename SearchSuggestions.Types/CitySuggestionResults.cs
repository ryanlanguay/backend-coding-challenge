namespace SearchSuggestions.Types
{
    using System.Collections.Generic;

    /// <summary>
    /// The results of a city suggestions search
    /// </summary>
    public class CitySuggestionResults
    {
        /// <summary>
        /// Gets or sets the suggestions
        /// </summary>
        public List<CitySuggestionResult> Suggestions { get; set; }
    }
}