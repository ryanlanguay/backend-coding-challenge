namespace SearchSuggestions.UnitTests
{
    using Data;
    using Microsoft.Extensions.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DataFileLoaderTests
    {
        private static IConfiguration Configuration;

        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            Configuration = ConfigurationHelper.BuildConfiguration();
        }

        [TestMethod]
        public void DataLoader_DataLoads()
        {
            var dataLoader = new DataFileLoader(Configuration);
            var cities = dataLoader.LoadData();
            Assert.IsTrue(cities.Count > 0);
        }
    }
}