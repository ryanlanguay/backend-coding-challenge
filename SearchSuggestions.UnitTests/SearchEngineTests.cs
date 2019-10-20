namespace SearchSuggestions.UnitTests
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Newtonsoft.Json;
    using SearchEngine;
    using Types;

    [TestClass]
    public class SearchEngineTests
    {
        private static CitySearchEngine searchEngine;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            var cityFileContent = File.ReadAllText("TestData.json");
            var cities = JsonConvert.DeserializeObject<List<City>>(cityFileContent);

            CityRepositoryMock.InitializeCities(cities);
            IndexedCityRepositoryMock.InitializeCities(cities);

            searchEngine = new CitySearchEngine(
                new CityRepositoryMock(), 
                new IndexedCityRepositoryMock(),
                new CityNameMatchScorer(), 
                new LocationMatchScorer());
        }

        #region No Location
        [TestMethod]
        public async Task Search_ValidText_1Result()
        {
            var searchText = "teau";
            var results = await searchEngine.Search(searchText, new LocationInformation(null, null));
            Assert.IsTrue(results.IsSuccess);
            Assert.AreEqual(1, results.Results.Suggestions.Count);
            Assert.AreEqual("Châteauguay, Quebec, CA", results.Results.Suggestions[0].Name);
        }

        [TestMethod]
        public async Task Search_TestNoAccent_NoResults()
        {
            var searchText = "Chateau";
            var results = await searchEngine.Search(searchText, new LocationInformation(null, null));
            Assert.IsTrue(results.IsSuccess);
            Assert.AreEqual(0, results.Results.Suggestions.Count);
        }

        [TestMethod]
        public async Task Search_ExactMatch_1Result()
        {
            var searchText = "Flin Flon";
            var results = await searchEngine.Search(searchText, new LocationInformation(null, null));
            Assert.IsTrue(results.IsSuccess);
            Assert.AreEqual(1, results.Results.Suggestions.Count);

            var match = results.Results.Suggestions[0];
            Assert.AreEqual("Flin Flon, Manitoba, CA", match.Name);
            Assert.AreEqual(0.85, match.Score);
        }

        [TestMethod]
        public async Task Search_ValidText_5Results()
        {
            var searchText = "lon";
            var results = await searchEngine.Search(searchText, new LocationInformation(null, null));
            Assert.IsTrue(results.IsSuccess);
            Assert.AreEqual(5, results.Results.Suggestions.Count);
            Assert.AreEqual("London, Ontario, CA", results.Results.Suggestions[0].Name);
            Assert.AreEqual("London, Kentucky, US", results.Results.Suggestions[1].Name);
            Assert.AreEqual(results.Results.Suggestions[0].Score, results.Results.Suggestions[1].Score);
            Assert.AreEqual("London, Ohio, US", results.Results.Suggestions[2].Name);
            Assert.AreEqual("Flin Flon, Manitoba, CA", results.Results.Suggestions[3].Name);
            Assert.AreEqual("Thurmont, Maryland, US", results.Results.Suggestions[4].Name);
        }

        [TestMethod]
        public async Task Search_ValidText_4Results()
        {
            var searchText = "flon";
            var results = await searchEngine.Search(searchText, new LocationInformation(null, null));
            Assert.IsTrue(results.IsSuccess);
            Assert.AreEqual(4, results.Results.Suggestions.Count);
            Assert.AreEqual("Flin Flon, Manitoba, CA", results.Results.Suggestions[0].Name);
            Assert.AreEqual("London, Ontario, CA", results.Results.Suggestions[1].Name);
            Assert.AreEqual("London, Kentucky, US", results.Results.Suggestions[2].Name);
            Assert.AreEqual("London, Ohio, US", results.Results.Suggestions[3].Name);

        }

        #endregion
        #region Location
        [TestMethod]
        public async Task Search_CanadaLocation_5Results()
        {
            var searchText = "lon";
            var results = await searchEngine.Search(searchText, new LocationInformation(43.70011, -79.4163));
            Assert.IsTrue(results.IsSuccess);
            Assert.AreEqual(5, results.Results.Suggestions.Count);
            Assert.AreEqual("London, Ontario, CA", results.Results.Suggestions[0].Name);
            Assert.AreEqual("London, Ohio, US", results.Results.Suggestions[1].Name);
            Assert.AreNotEqual(results.Results.Suggestions[0].Score, results.Results.Suggestions[1].Score);
            Assert.AreEqual("London, Kentucky, US", results.Results.Suggestions[2].Name);
            Assert.AreEqual("Flin Flon, Manitoba, CA", results.Results.Suggestions[3].Name);
            Assert.AreEqual("Thurmont, Maryland, US", results.Results.Suggestions[4].Name);
        }

        [TestMethod]
        public async Task Search_USLocation_5Results()
        {
            var searchText = "lon";
            var results = await searchEngine.Search(searchText, new LocationInformation(37, -84));
            Assert.IsTrue(results.IsSuccess);
            Assert.AreEqual(5, results.Results.Suggestions.Count);
            Assert.AreEqual("London, Kentucky, US", results.Results.Suggestions[0].Name);
            Assert.AreEqual("London, Ohio, US", results.Results.Suggestions[1].Name);
            Assert.AreEqual("London, Ontario, CA", results.Results.Suggestions[2].Name);
            Assert.AreEqual("Flin Flon, Manitoba, CA", results.Results.Suggestions[3].Name);
            Assert.AreEqual("Thurmont, Maryland, US", results.Results.Suggestions[4].Name);
        }
        #endregion
        #region Errors
        [TestMethod]
        public async Task Search_NoText_Error()
        {
            var results = await searchEngine.Search(null, new LocationInformation(null, null));
            Assert.IsFalse(results.IsSuccess);

            results = await searchEngine.Search(string.Empty, new LocationInformation(null, null));
            Assert.IsFalse(results.IsSuccess);

            results = await searchEngine.Search(" ", new LocationInformation(null, null));
            Assert.IsFalse(results.IsSuccess);
        }

        [TestMethod]
        public async Task Search_TooShortText_Error()
        {
            var searchText = "a";
            var results = await searchEngine.Search(searchText, new LocationInformation(null, null));
            Assert.IsFalse(results.IsSuccess);
        }

        [TestMethod]
        public async Task Search_InvalidLocation_Error()
        {
            var searchText = "Londo";
            var location = new LocationInformation(1.0d, null);
            var results = await searchEngine.Search(searchText, location);
            Assert.IsFalse(results.IsSuccess);

            location = new LocationInformation(null, 1.0d);
            results = await searchEngine.Search(searchText, location);
            Assert.IsFalse(results.IsSuccess);
        }

        [TestMethod]
        public async Task Search_InvalidLatitudesAndLongitudes_Error()
        {
            var searchText = "Londo";
            var location = new LocationInformation(-91, 0);
            var results = await searchEngine.Search(searchText, location);
            Assert.IsFalse(results.IsSuccess);

            location = new LocationInformation(91, 0);
            results = await searchEngine.Search(searchText, location);
            Assert.IsFalse(results.IsSuccess);

            location = new LocationInformation(0, -181);
            results = await searchEngine.Search(searchText, location);
            Assert.IsFalse(results.IsSuccess);

            location = new LocationInformation(0, 181);
            results = await searchEngine.Search(searchText, location);
            Assert.IsFalse(results.IsSuccess);
        }

        #endregion
    }
}