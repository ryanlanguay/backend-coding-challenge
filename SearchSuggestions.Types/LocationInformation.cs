namespace SearchSuggestions.Types
{
    /// <summary>
    /// Information about a user's location
    /// </summary>
    public class LocationInformation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LocationInformation" /> class
        /// </summary>
        /// <param name="latitude">The latitude</param>
        /// <param name="longitude">The longitude</param>
        public LocationInformation(double? latitude, double? longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }

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