using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using TW.Utility.Extension;
using UnityEngine;

namespace SDK
{
    public class Static
    {
        public static async Task<List<Dictionary<string, string>>> GetCSVDataFromGoogleSheet(string url, string sheet)
        {
            string dataText = await ABakingSheet.GetCsv(url, sheet);
            using var reader = new StringReader(dataText);
            using var csv = new CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture));
            var records = new List<Dictionary<string, string>>();
            csv.Read();
            csv.ReadHeader();

            while (csv.Read())
            {
                var record = new Dictionary<string, string>();
                foreach (var header in csv.HeaderRecord)
                {
                    record[header] = csv.GetField(header);
                }

                records.Add(record);
            }

            return records;
        }
    }
}
