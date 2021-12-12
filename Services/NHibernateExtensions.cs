using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Microsoft.Extensions.DependencyInjection;
using PGManagerApi.Mappings;
using PGManagerApi.Settings;

namespace PGManagerApi.Services
{
    public static class NHibernateExtensions
    {
        public static IServiceCollection AddNHibernate(this IServiceCollection services, DatabaseSettings databaseSettings)
        {
            var nhConfiguration = Fluently.Configure();
            nhConfiguration.Database(PostgreSQLConfiguration.Standard
                .ConnectionString(c => {
                    c.Host(databaseSettings.Host);
                    c.Port(databaseSettings.Port);
                    c.Database(databaseSettings.Database);
                    c.Username(databaseSettings.Username);
                    c.Password(databaseSettings.Password);
                }));

            nhConfiguration.Mappings(m => m.FluentMappings.AddFromAssemblyOf<UserDatabaseMapping>());

            var sessionFactory = nhConfiguration.BuildSessionFactory();

            services.AddSingleton(sessionFactory);

            return services;
        }
    }
}