namespace SearchSuggestions.UnitTests
{
    using Data;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DataFileLoaderTests
    {
        [TestMethod]
        public void DataLoader_DataLoads()
        {
            var dataLoader = new DataFileLoader();
            dataLoader.LoadData();
            Assert.IsTrue(dataLoader.Cities.Count > 0);
        }
    }
}