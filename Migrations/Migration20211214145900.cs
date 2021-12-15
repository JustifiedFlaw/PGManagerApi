using FluentMigrator;

namespace PGManagerApi.Migrations
{
    [Migration(20211214145900)]
    public class MakeIdPrimaryKeyInConnections : Migration
    {
        public override void Down()
        {
            this.Create.PrimaryKey()
                .OnTable("databaseconnections")
                .Columns("username", "connectionname");
        }

        public override void Up()
        {
            this.Delete.PrimaryKey("PK_userdatabases")
                .FromTable("databaseconnections");

            this.Create.PrimaryKey()
                .OnTable("databaseconnections")
                .Column("id");
        }
    }
}