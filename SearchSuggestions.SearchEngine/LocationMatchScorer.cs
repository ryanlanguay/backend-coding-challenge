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

                var (x1, y1) = received;
                var (x2, y2) = actual;

                var distance = MathHelper.GetCartesianDistance((x1, y1), (x2, y2));
                
                // The shorter the distance, the closer the match, and the higher the score
                if (distance == 0)
                    return 1;

                return (1 / distance) * SearchScoringFactors.LocationScoringFactor;
            });
        }
    }
}