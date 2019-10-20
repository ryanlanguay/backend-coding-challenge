namespace SearchSuggestions.SearchEngine
{
    using System;
    using System.Linq;

    internal static class MathHelper
    {
        public static double GetPercentDifference(double expected, double actual)
        {
            return Math.Abs(expected - actual) / Math.Abs(Average(expected, actual));
        }

        public static double GetBoundedValue(double min, double val, double max)
        {
            return Math.Max(min, Math.Min(val, max));
        }

        public static double Average(double a, double b)
        {
            var items = new[] {a, b};
            return items.Average();
        }

        public static bool IsInRange(double min, double val, double max)
        {
            return val >= min && val <= max;
        }
    }
}