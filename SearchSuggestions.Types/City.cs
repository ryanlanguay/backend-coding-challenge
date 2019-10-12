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
        /// Gets or sets the city latitude
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the city longitude
        /// </summary>
        public double Longitude { get; set; }
    }
}