namespace SearchSuggestions.SearchEngine
{
    using System;
    using System.Linq;

    /// <summary>
    /// Helper class to perform common math operations
    /// </summary>
    internal static class MathHelper
    {
        /// <summary>
        /// Gets the percent difference between two numbers
        /// </summary>
        /// <param name="expected">The expected value</param>
        /// <param name="actual">The actual value</param>
        /// <returns>The percent difference</returns>
        public static double GetPercentDifference(double expected, double actual)
        {
            return Math.Abs(expected - actual) / Math.Abs(Average(expected, actual));
        }

        /// <summary>
        /// Gets a value, bounded by a range
        /// </summary>
        /// <remarks>
        /// If the value is within the range [min, max], it is returned
        /// If the value is less than the minimum, the minimum is returned
        /// If the value is greater than the maximum, the maximum is returned
        /// </remarks>
        /// <param name="min">The minimum value</param>
        /// <param name="val">The value</param>
        /// <param name="max">The maximum value</param>
        /// <returns>The bounded value</returns>
        public static double GetBoundedValue(double min, double val, double max)
        {
            return Math.Max(min, Math.Min(val, max));
        }

        /// <summary>
        /// Gets the average of two numbers
        /// </summary>
        /// <param name="a">The first number</param>
        /// <param name="b">The second number</param>
        /// <returns>The average</returns>
        public static double Average(double a, double b)
        {
            var items = new[] {a, b};
            return items.Average();
        }

        /// <summary>
        /// Returns true if a value is in a given (inclusive) range
        /// </summary>
        /// <param name="min">The lower bound of the range</param>
        /// <param name="val">The value to check</param>
        /// <param name="max">The upper bound of the range</param>
        /// <returns></returns>
        public static bool IsInRange(double min, double val, double max)
        {
            return val >= min && val <= max;
        }
    }
}