namespace SearchSuggestions.SearchEngine
{
    using System;
    using System.Threading.Tasks;
    using Types;

    /// <summary>
    /// Scoring class for location information
    /// <see cref="IScorer{T}"/>
    /// </summary>
    public class LocationMatchScorer : IScorer<LocationInformation>
    {
        /// <inheritdoc cref="IScorer{T}" />
        public Task<double> GetMatchScore(LocationInformation received, LocationInformation actual)
        {
            return Task.Run(() =>
            {
                if (!actual.Latitude.HasValue || !actual.Longitude.HasValue)
                {
                    // This is an error state; the data from the database is missing
                    throw new ArgumentOutOfRangeException(nameof(actual));
                }

                if (!received.Latitude.HasValue && !received.Longitude.HasValue)
                {
                    // We received no location information; no bonus scoring used
                    return 0.0d;
                }

                var latitudeDifference = MathHelper.GetPercentDifference(received.Latitude.Value, actual.Latitude.Value);
                var longitudeDifference = MathHelper.GetPercentDifference(received.Longitude.Value, actual.Longitude.Value);

                var totalSimilarity = 1 - latitudeDifference - longitudeDifference;
                return Math.Max(totalSimilarity, 0) * SearchScoringFactors.LocationScoringFactor;
            });
        }
    }
}