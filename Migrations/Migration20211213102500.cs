using FluentMigrator;

namespace WrBotApi.Migrations
{
    [Migration(20211213102500)]
    public class RenameUserDatabaseToDatabaseConnection : Migration
    {
        public override void Down()
        {
            this.Rename.Table("databaseconnections")
                .To("userdatabases");

            this.Rename.Column("connectionname")
                .OnTable("databaseconnections")
                .To("databasename");
        }

        public override void Up()
        {
            this.Rename.Table("userdatabases")
                .To("databaseconnections");

            this.Rename.Column("databasename")
                .OnTable("databaseconnections")
                .To("connectionname");
        }
    }
}