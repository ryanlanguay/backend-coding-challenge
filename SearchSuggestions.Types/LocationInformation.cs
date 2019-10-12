namespace SearchSuggestions.Types
{
    /// <summary>
    /// Information about a user's location
    /// </summary>
    public class LocationInformation
    {
        /// <summary>
        /// Gets or sets the latitude
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude
        /// </summary>
        public double? Longitude { get; set; }
    }
}