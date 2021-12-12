using FluentNHibernate.Mapping;
using PGManagerApi.Models;

namespace PGManagerApi.Mappings
{
    public class UserDatabaseMapping : ClassMap<UserDatabase>
    {
        public UserDatabaseMapping()
        {
            this.Table("userdatabases");

            this.CompositeId()
                .KeyProperty(x => x.Username)
                .KeyProperty(x => x.DatabaseName);

            this.Map(x => x.ConnectionHost);
            this.Map(x => x.ConnectionPort);
            this.Map(x => x.ConnectionDatabase);
            this.Map(x => x.ConnectionUsername);
            this.Map(x => x.ConnectionPassword);
        }
    }
}