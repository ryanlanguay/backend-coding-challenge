namespace SearchSuggestions.SearchEngine
{
    using System;
    using System.Linq;
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

                var latitudeDiff = expected.Latitude.HasValue
                    ? GetPercentageDifference(expected.Latitude.Value, actual.Latitude.Value)
                    : 0.0d;

                var longitudeDiff = expected.Longitude.HasValue
                    ? GetPercentageDifference(expected.Longitude.Value, actual.Longitude.Value)
                    : 0.0d;

                return (latitudeDiff + longitudeDiff) / 2.0d;
            });
        }

        private static double GetPercentageDifference(double a, double b)
        {
            return Math.Abs(a - b) / (new[] {a, b}.Average());
        }
    }
}