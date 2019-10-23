namespace SearchSuggestions.UnitTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test class to validate n-gram parser
    /// </summary>
    [TestClass]
    public class NGramParserTests
    {
        /// <summary>
        /// A valid city name
        /// </summary>
        private const string ValidCityName = "London";

        /// <summary>
        /// Parses a valid city name into n-grams of length 2 and 3
        /// </summary>
        /// <returns>Void</returns>
        [TestMethod]
        public async Task Parse_23_ValidResults()
        {
            var nGrams = await StringNGramParser.GetNGrams(ValidCityName, 2, 3);
            Assert.AreEqual(9, nGrams.Length);
            Assert.AreEqual(5, nGrams.Count(n => n.Length == 2));
            Assert.AreEqual(4, nGrams.Count(n => n.Length == 3));
        }

        /// <summary>
        /// Parses a valid city name into n-grams of length 2, 3, and 4
        /// </summary>
        /// <returns>Void</returns>
        [TestMethod]
        public async Task Parse_24_ValidResults()
        {
            var nGrams = await StringNGramParser.GetNGrams(ValidCityName, 2, 4);
            Assert.AreEqual(12, nGrams.Length);
            Assert.AreEqual(5, nGrams.Count(n => n.Length == 2));
            Assert.AreEqual(4, nGrams.Count(n => n.Length == 3));
            Assert.AreEqual(3, nGrams.Count(n => n.Length == 4));
        }

        /// <summary>
        /// Error case: minimum n-gram length is 0
        /// </summary>
        /// <returns>Void</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task Parse_Min0()
        {
            var nGrams = await StringNGramParser.GetNGrams(ValidCityName, 0, 3);
        }

        /// <summary>
        /// Error case: minimum n-gram length is 1
        /// </summary>
        /// <returns>Void</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task Parse_Min1()
        {
            var nGrams = await StringNGramParser.GetNGrams(ValidCityName, 1, 3);
        }

        /// <summary>
        /// Error case: max length is greater than string length
        /// </summary>
        /// <returns>Void</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task Parse_MaxTooLong()
        {
            var nGrams = await StringNGramParser.GetNGrams(ValidCityName, 2, 7);
        }

        /// <summary>
        /// Error case: minimum value greater than maximum
        /// </summary>
        /// <returns>Void</returns>
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task Parse_MinGreaterThanMax()
        {
            var nGrams = await StringNGramParser.GetNGrams(ValidCityName, 3, 2);
        }
    }
}