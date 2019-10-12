namespace SearchSuggestions.Data
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using Aspose.Cells;
    using Types;

    public class DataFileLoader
    {
        private const string filePath = @"Data\cities_canada-usa.tsv";

        public List<City> Cities { get; private set; }

        public void LoadData()
        {
            this.Cities = new List<City>();

            var dataFilePath = Path.Combine(Directory.GetCurrentDirectory(), filePath);
            if (!File.Exists(dataFilePath))
            {
                throw new FileNotFoundException("The data file could not be found!", dataFilePath);
            }

            var workbook = new Workbook(dataFilePath);
            var dataCells = workbook.Worksheets.First().Cells;

            var data = dataCells.ExportDataTable(0, 0, dataCells.Rows.Count, dataCells.Columns.Count);

            foreach (DataRow dataRow in data.Rows)
            {
                var city = new City
                {
                    Id = Convert.ToInt64(dataRow["id"]),
                    Name = dataRow["name"].ToString(),
                    Latitude = Convert.ToDouble(dataRow["latitude"]),
                    Longitude = Convert.ToDouble(dataRow["longitude"])
                };

                this.Cities.Add(city);
            }
        }
    }
}