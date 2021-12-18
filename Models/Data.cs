using System.Collections.Generic;
using System.Data;

namespace PGManagerApi.Models
{
    public class Data
    {
        public FieldTypes FieldTypes { get; set; } = new FieldTypes();
        public List<Row> Rows { get; set; } = new List<Row>();

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