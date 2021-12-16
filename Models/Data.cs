using System.Collections.Generic;
using System.Data;

namespace PGManagerApi.Models
{
    public class Data
    {
        public Dictionary<string, string> FieldTypes { get; set; } = new Dictionary<string, string>();
        public List<Dictionary<string, object>> Rows { get; set; } = new List<Dictionary<string, object>>();

        public Data()
        {
        }

        public Data(IDataReader reader)
        {
            while (reader.Read())
            {
                if (this.FieldTypes.Count == 0)
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        this.FieldTypes.Add(reader.GetName(i), reader.GetDataTypeName(i));
                    }
                }

                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row.Add(reader.GetName(i), reader[i]);
                }
                this.Rows.Add(row);
            }
        }
    }
}