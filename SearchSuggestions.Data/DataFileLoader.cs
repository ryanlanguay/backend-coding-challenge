namespace SearchSuggestions.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Microsoft.Extensions.Configuration;
    using Types;

    /// <summary>
    /// Class used to load data from data files
    /// </summary>
    public class DataFileLoader
    {
        /// <summary>
        /// The application configuration
        /// </summary>
        private readonly IConfiguration config;

        /// <summary>
        /// The state code / name mapping (from the data file)
        /// </summary>
        private Dictionary<string, string> stateCodeMapping;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataFileLoader" /> class
        /// </summary>
        /// <param name="config">The application configuration</param>
        public DataFileLoader(IConfiguration config)
        {
            this.config = config;
        }

        /// <summary>
        /// Loads the city data from files
        /// </summary>
        /// <returns>The cities</returns>
        public List<City> LoadData()
        {
            this.LoadStateData();

            var filePath = this.config[Constants.FilePathKey];
            var dataParser = new TabSeparatedDataParser(filePath);
            var data = dataParser.ParseData();

            return (from DataRow dataRow in data.Rows
                select new City
                {
                    Id = Convert.ToInt64(dataRow["id"]),
                    Name = dataRow["name"].ToString(),
                    LocationInformation = new LocationInformation(Convert.ToDouble(dataRow["lat"]), Convert.ToDouble(dataRow["long"])),
                    RegionName = this.stateCodeMapping[$"{dataRow["country"]}.{dataRow["admin1"]}"],
                    CountryCode = dataRow["country"].ToString()
                }).ToList();
        }

        /// <summary>
        /// Loads state data
        /// </summary>
        private void LoadStateData()
        {
            this.stateCodeMapping = new Dictionary<string, string>();

            var adminCode1Path = this.config[Constants.StateFilePathKey];
            var dataParser = new TabSeparatedDataParser(adminCode1Path);
            var data = dataParser.ParseData();

            foreach (DataRow dataRow in data.Rows)
            {
                var geoNameCode = dataRow["code"].ToString();
                var name = dataRow["name"].ToString();

                this.stateCodeMapping.Add(geoNameCode, name);
            }
        }
    }
}