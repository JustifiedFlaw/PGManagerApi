using System.Collections.Generic;
using System.Data;

namespace PGManagerApi.Models
{
    public class Data : List<Dictionary<string, object>>
    {
        public Data()
        {
        }

        public Data(IDataReader reader)
        {
            while (reader.Read())
            {
                var row = new Dictionary<string, object>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row.Add(reader.GetName(i), reader[i]);
                }
                this.Add(row);
            }
        }
    }
}