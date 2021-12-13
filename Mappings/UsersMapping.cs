using FluentNHibernate.Mapping;
using PGManagerApi.Models;

namespace WrBotApi.Mappings
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