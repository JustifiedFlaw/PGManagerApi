using System.Collections.Generic;
using System.Data;

namespace PGManagerApi.Models
{
    public class Data
    {
        public List<Row> Rows { get; set; } = new List<Row>();

        public Data()
        {
        }

        public Data(IDataReader reader)
        {
            while (reader.Read())
            {
                var row = new Row();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row.Add(reader.GetName(i), reader[i]);
                }
                this.Rows.Add(row);
            }
        }
    }
}