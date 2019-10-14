namespace SearchSuggestions.UnitTests
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class NGramParserTests
    {
        private const string ValidCityName = "London";

        [TestMethod]
        public async Task Parse_23_ValidResults()
        {
            var nGrams = await StringNGramParser.GetNGrams(ValidCityName, 2, 3);
            Assert.AreEqual(9, nGrams.Length);
            Assert.AreEqual(5, nGrams.Count(n => n.Length == 2));
            Assert.AreEqual(4, nGrams.Count(n => n.Length == 3));
        }

        [TestMethod]
        public async Task Parse_24_ValidResults()
        {
            var nGrams = await StringNGramParser.GetNGrams(ValidCityName, 2, 4);
            Assert.AreEqual(12, nGrams.Length);
            Assert.AreEqual(5, nGrams.Count(n => n.Length == 2));
            Assert.AreEqual(4, nGrams.Count(n => n.Length == 3));
            Assert.AreEqual(3, nGrams.Count(n => n.Length == 4));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task Parse_Min0()
        {
            var nGrams = await StringNGramParser.GetNGrams(ValidCityName, 0, 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task Parse_Min1()
        {
            var nGrams = await StringNGramParser.GetNGrams(ValidCityName, 1, 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task Parse_MaxTooLong()
        {
            var nGrams = await StringNGramParser.GetNGrams(ValidCityName, 2, 7);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public async Task Parse_MinGreaterThanMax()
        {
            var nGrams = await StringNGramParser.GetNGrams(ValidCityName, 3, 2);
        }
    }
}