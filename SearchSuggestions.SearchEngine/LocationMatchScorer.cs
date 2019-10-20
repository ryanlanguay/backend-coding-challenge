namespace SearchSuggestions.SearchEngine
{
    using System;
    using System.Threading.Tasks;
    using Types;

    public class LocationMatchScorer : IScorer<LocationInformation>
    {
        public Task<double> GetMatchScore(LocationInformation expected, LocationInformation actual)
        {
            return Task.Run(() =>
            {
                if (!actual.Latitude.HasValue || !actual.Longitude.HasValue)
                {
                    // This is an error state; the data from the database is missing
                    throw new ArgumentOutOfRangeException(nameof(actual));
                }

                if (!expected.Latitude.HasValue && !expected.Longitude.HasValue)
                {
                    return 0.0d;
                }

                var latitudeError = MathHelper.GetPercentDifference(expected.Latitude.Value, actual.Latitude.Value);
                var longitudeError = MathHelper.GetPercentDifference(expected.Longitude.Value, actual.Longitude.Value);

                var totalSimilarity = 1 - latitudeError - longitudeError;
                return Math.Max(totalSimilarity, 0) * SearchScoringFactors.LocationScoringFactor;
            });
        }
    }
}