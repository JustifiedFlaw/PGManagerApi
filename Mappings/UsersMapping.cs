using FluentNHibernate.Mapping;
using PGManagerApi.Models;

namespace PGManagerApi.Mappings
{
    public class UserMapping : ClassMap<User>
    {
        public UserMapping()
        {
            this.Table("users");
            this.Id(x => x.Name);
            this.Map(x => x.PasswordHash);
        }
    }
}