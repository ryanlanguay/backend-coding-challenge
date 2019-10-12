namespace SearchSuggestions.Types
{
    /// <summary>
    /// Search criteria for a city
    /// </summary>
    public class CitySearchCriteria
    {
        /// <summary>
        /// Gets or sets the query string
        /// </summary>
        public string Query { get; set; }

        /// <summary>
        /// Gets or sets the location information
        /// </summary>
        public LocationInformation LocationInformation { get; set; }
    }
}