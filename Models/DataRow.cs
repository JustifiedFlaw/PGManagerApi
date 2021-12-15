using System.Collections.Generic;

namespace PGManagerApi.Models
{
    public class DataRow
    {
        public Dictionary<string, object> Columns { get; set; } = new Dictionary<string, object>();
    }
}