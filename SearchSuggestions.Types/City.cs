namespace SearchSuggestions.Types
{
    /// <summary>
    /// City information from database
    /// </summary>
    public class City
    {
        /// <summary>
        /// Gets or sets the city unique ID
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Gets or sets the city name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the city's location information
        /// </summary>
        public LocationInformation LocationInformation { get; set; }

        /// <summary>
        /// Gets or sets the region (state/province) name
        /// </summary>
        public string RegionName { get; set; }

        /// <summary>
        /// Gets or sets the country code
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the display name of the city
        /// </summary>
        public string DisplayName => $"{this.Name}, {this.RegionName}, {this.CountryCode}";
    }
}