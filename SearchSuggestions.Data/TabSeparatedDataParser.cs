using System;
using System.Collections.Generic;
using System.Text;

namespace SearchSuggestions.Data
{
    using System.Data;
    using System.IO;
    using System.Runtime.CompilerServices;

    internal class TabSeparatedDataParser
    {
        private readonly string filePath;

        public TabSeparatedDataParser(string filePath)
        {
            this.filePath = filePath;
        }

        public DataTable ParseData()
        {
            if (!File.Exists(this.filePath) || !this.filePath.EndsWith(".tsv"))
                throw new FileNotFoundException(".tsv file not found!", this.filePath);

            var dataTable = new DataTable();

            var fileContent = File.ReadAllLines(this.filePath);

            var headers = fileContent[0].Split('\t');
            foreach (var header in headers)
            {
                dataTable.Columns.Add(header);
            }

            for (var i = 1; i < fileContent.Length; i++)
            {
                var row = dataTable.NewRow();
                var dataColumns = fileContent[i].Split('\t');
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
