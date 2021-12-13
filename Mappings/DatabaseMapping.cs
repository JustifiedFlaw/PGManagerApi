using FluentNHibernate.Mapping;
using PGManagerApi.Models;

namespace PGManagerApi.Mappings
{
    public class DatabaseMapping : ClassMap<Database>
    {
        public DatabaseMapping()
        {
            this.Table("pg_database");

            this.Id(x => x.Id, "oid");
            this.Map(x => x.Name, "datname");
            
            this.Where("datistemplate = false");
        }
    }
}