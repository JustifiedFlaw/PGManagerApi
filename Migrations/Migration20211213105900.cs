using FluentMigrator;

namespace PGManagerApi.Migrations
{
    [Migration(20211213105900)]
    public class AddIdToDatabaseConnections : Migration
    {
        public override void Down()
        {
            this.Delete.Column("id")
                .FromTable("databaseconnections");
        }

        public override void Up()
        {
            this.Create.Column("id")
                .OnTable("databaseconnections")
                .AsInt64().Identity().PrimaryKey(); // TODO: did not change PK
            
            this.Create.Index()
                .OnTable("databaseconnections")
                .OnColumn("username").Ascending()
                .OnColumn("connectionname").Ascending();
        }
    }
}