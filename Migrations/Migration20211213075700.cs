using FluentMigrator;

namespace PGManagerApi.Migrations
{
    [Migration(20211213075700)]
    public class AddUserTable : Migration
    {
        public override void Down()
        {
            this.Delete.Table("users");
        }

        public override void Up()
        {
            this.Create.Table("users")
                .WithColumn("name").AsAnsiString().PrimaryKey()
                .WithColumn("passwordhash").AsAnsiString();
        }
    }
}