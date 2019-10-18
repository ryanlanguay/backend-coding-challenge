namespace SearchSuggestions.SearchEngine
{
    using System;

    internal static class MathHelper
    {
        public static double GetPercentError(double expected, double actual)
        {
            return Math.Abs(expected - actual) / Math.Abs(actual);
        }

        public static double GetBoundedValue(double min, double val, double max)
        {
            return Math.Max(min, Math.Min(val, max));
        }
    }
}