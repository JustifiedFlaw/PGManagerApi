using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using PGManagerApi.Migrations;
using PGManagerApi.Settings;

namespace PGManagerApi.Services
{
    public static class DatabaseMigrator
    {
        public static void Migrate(DatabaseSettings databaseSettings)
        {
            using (var serviceProvider = new ServiceCollection()
                .AddFluentMigratorCore()
                .ConfigureRunner(rb => rb
                    .AddPostgres()
                    .WithGlobalConnectionString(databaseSettings.ConnectionString)
                    .ScanIn(typeof(AddUserDatabasesTable).Assembly).For.Migrations())
                .AddLogging(lb => lb.AddFluentMigratorConsole())
                .BuildServiceProvider(false))
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var runner = serviceProvider.GetRequiredService<IMigrationRunner>();
                    runner.MigrateUp();
                }
            }
        }
    }
}