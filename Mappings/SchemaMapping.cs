using FluentNHibernate.Mapping;
using PGManagerApi.Models;

namespace PGManagerApi.Mappings
{
    public class SchemaMapping : ClassMap<Schema>
    {
        public SchemaMapping()
        {
            this.Schema("pg_catalog");
            this.Table("pg_namespace");

            this.Id(x => x.Id, "oid");
            
            this.Map(x => x.Name, "nspname");
        }
    }
}