﻿namespace SearchSuggestions.Types
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// A city suggestion result
    /// </summary>
    public class CitySuggestionResult
    {
        /// <summary>
        /// Gets or sets the name of the city
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the latitude of the city
        /// </summary>
        public double Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the city
        /// </summary>
        public double Longitude { get; set; }

        /// <summary>
        /// Gets or sets the search score
        /// </summary>
        [Range(0.0, 1.0)]
        public double Score { get; set; }
    }
}