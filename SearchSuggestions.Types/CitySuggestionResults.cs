namespace SearchSuggestions.Types
{
    using System.Collections.Generic;

    /// <summary>
    /// The results of a city suggestions search
    /// </summary>
    public class CitySuggestionResults
    {
        /// <summary>
        /// Initializes a default instance of the <see cref="CitySuggestionResults" /> class
        /// </summary>
        public CitySuggestionResults()
        {
            this.Suggestions = new List<CitySuggestionResult>();
        }

        /// <summary>
        /// Gets or sets the suggestions
        /// </summary>
        public List<CitySuggestionResult> Suggestions { get; set; }
    }
}