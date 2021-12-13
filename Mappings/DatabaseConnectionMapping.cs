using FluentNHibernate.Mapping;
using PGManagerApi.Models;

namespace PGManagerApi.Mappings
{
    public class DatabaseConnectionMapping : ClassMap<DatabaseConnection>
    {
        public DatabaseConnectionMapping()
        {
            this.Table("databaseconnections");

            this.Id(x => x.Id).GeneratedBy.Increment();

            this.Map(x => x.Username);
            this.Map(x => x.ConnectionName);
            this.Map(x => x.ConnectionHost);
            this.Map(x => x.ConnectionPort);
            this.Map(x => x.ConnectionDatabase);
            this.Map(x => x.ConnectionUsername);
            this.Map(x => x.ConnectionPassword);
        }
    }
}