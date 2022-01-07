using FluentMigrator;

namespace PGManagerApi.Migrations
{
    [Migration(20220107063100)]
    public class AddDefaultUser : Migration
    {
        public override void Down()
        {
            this.Delete.FromTable("users")
                .Row(new 
                {
                    name = "PGManager"
                });
        }

        public override void Up()
        {
            this.Insert.IntoTable("users")
                .Row(new 
                {
                    name = "PGManager",
                    passwordhash = BCrypt.Net.BCrypt.HashPassword("PGManager")
                });
        }
    }
}