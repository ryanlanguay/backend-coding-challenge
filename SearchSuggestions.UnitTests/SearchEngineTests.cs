namespace SearchSuggestions.UnitTests
{
    using System.Collections.Generic;
    using System.IO;
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

        [TestMethod]
        public async Task Search_ValidText_Results()
        {
            var searchText = "Londo";
            var results = await searchEngine.Search(searchText, new LocationInformation(null, null));
            Assert.IsTrue(results.IsSuccess);
            Assert.AreEqual(2, results.Results.Suggestions.Count);
        }

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
    }
}