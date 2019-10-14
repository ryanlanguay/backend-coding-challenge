namespace SearchSuggestions.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Types;

    public class DataFileLoader
    {
        private const string filePath = @"Data\cities_canada-usa.tsv";
        private const string adminCode1Path = @"Data\admin1CodesASCII.tsv";
        private Dictionary<string, string> stateCodeMapping;
        
        public List<City> Cities { get; private set; }

        public void LoadData()
        {
            this.LoadStateData();

            this.Cities = new List<City>();
            
            var dataParser = new TabSeparatedDataParser(filePath);
            var data = dataParser.ParseData();

            foreach (DataRow dataRow in data.Rows)
            {
                var city = new City
                {
                    Id = Convert.ToInt64(dataRow["id"]),
                    Name = dataRow["name"].ToString(),
                    Latitude = Convert.ToDouble(dataRow["lat"]),
                    Longitude = Convert.ToDouble(dataRow["long"]),
                    RegionName = this.stateCodeMapping[$"{dataRow["country"]}.{dataRow["admin1"]}"]
                };

                this.Cities.Add(city);
            }
        }

        private void LoadStateData()
        {
            this.stateCodeMapping = new Dictionary<string, string>();

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