using System.Collections.Generic;
using System.Data;
using Npgsql;

namespace PGManagerApi.Models
{
    public class Data
    {
        public List<DataRow> Rows { get; set; } = new List<DataRow>();

        public Data(IDataReader reader)
        {
            while (reader.Read())
            {
                var row = new DataRow();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row.Columns.Add(reader.GetName(i), reader[i]);
                }
                this.Rows.Add(row);
            }
        }
    }
}