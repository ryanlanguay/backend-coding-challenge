namespace SearchSuggestions.Data
{
    using System;
    using System.Data;
    using System.IO;
    using System.Linq;

    /// <summary>
    /// Helper class used to parse tab-separated data
    /// </summary>
    internal class TabSeparatedDataParser
    {
        /// <summary>
        /// The tab character
        /// </summary>
        private const char TabSeparator = '\t';

        /// <summary>
        /// The file path to read from
        /// </summary>
        private readonly string filePath;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabSeparatedDataParser" /> class
        /// </summary>
        /// <param name="filePath">The file path</param>
        public TabSeparatedDataParser(string filePath)
        {
            this.filePath = Path.Combine(AppContext.BaseDirectory, filePath);
        }

        /// <summary>
        /// Parses tab-separated data into a data table
        /// </summary>
        /// <returns>The data table</returns>
        public DataTable ParseData()
        {
            if (!File.Exists(this.filePath) || !this.filePath.EndsWith(".tsv"))
                throw new FileNotFoundException(".tsv file not found!", this.filePath);

            var dataTable = new DataTable();

            var fileContent = File.ReadAllLines(this.filePath);

            if (!fileContent.Any())
                throw new FileNotFoundException(".tsv file is empty!", this.filePath);

            var headers = fileContent[0].Split(TabSeparator);
            foreach (var header in headers)
            {
                dataTable.Columns.Add(header);
            }

            for (var i = 1; i < fileContent.Length; i++)
            {
                var row = dataTable.NewRow();
                var dataColumns = fileContent[i].Split(TabSeparator);
                for (var j = 0; j < dataTable.Columns.Count; j++)
                {
                    row[j] = dataColumns[j];
                }

                dataTable.Rows.Add(row);
            }

            dataTable.AcceptChanges();
            return dataTable;
        }
    }
}