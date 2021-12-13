using FluentNHibernate.Mapping;
using PGManagerApi.Models;

namespace PGManagerApi.Mappings
{
    public class TableMapping : ClassMap<Table>
    {
        public TableMapping()
        {
            this.Schema("pg_catalog");
            this.Table("pg_tables");

            this.CompositeId()
                .KeyProperty(x => x.SchemaName)
                .KeyProperty(x => x.TableName);
            
            this.Where("schemaname != 'pg_catalog' AND schemaname != 'information_schema'");
        }
    }
}