using FluentMigrator;

namespace PGManagerApi.Migrations
{
    [Migration(20121212135800)]
    public class AddUserDatabasesTable : Migration
    {
        public override void Down()
        {
            this.Delete.Table("userdatabases");
        }

        public override void Up()
        {
            this.Create.Table("userdatabases")
                .WithColumn("username").AsAnsiString().PrimaryKey()
                .WithColumn("databasename").AsAnsiString().PrimaryKey()
                .WithColumn("connectionhost").AsAnsiString()
                .WithColumn("connectionport").AsInt16()
                .WithColumn("connectiondatabase").AsAnsiString()
                .WithColumn("connectionusername").AsAnsiString()
                .WithColumn("connectionpassword").AsAnsiString();            
        }
    }
}