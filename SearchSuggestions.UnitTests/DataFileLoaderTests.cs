namespace SearchSuggestions.UnitTests
{
    using System.Linq;
    using Data;
    using Microsoft.Extensions.Configuration;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Test methods to ensure the data files are properly loaded
    /// </summary>
    [TestClass]
    public class DataFileLoaderTests
    {
        /// <summary>
        /// The configuration for the test
        /// </summary>
        private static IConfiguration Configuration;

        /// <summary>
        /// Initializes the test class
        /// </summary>
        /// <param name="context">The test class</param>
        [ClassInitialize]
        public static void Initialize(TestContext context)
        {
            Configuration = ConfigurationHelper.BuildConfiguration();
        }

        /// <summary>
        /// Ensure the DataLoader properly loads the city data file
        /// </summary>
        [TestMethod]
        public void DataLoader_DataLoads()
        {
            var dataLoader = new DataFileLoader(Configuration);
            var cities = dataLoader.LoadData();
            Assert.IsTrue(cities.Count > 0);
            Assert.IsTrue(!cities.Any(c => c == null || c.Id == default));
        }
    }
}