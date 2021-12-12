using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using PGManagerApi.Exceptions;
using PGManagerApi.Mappings;
using PGManagerApi.Models;

namespace PGManagerApi.Services
{
    public class UserDatabaseService
    {
        ISessionFactory SessionFactory;

        public UserDatabaseService(ISessionFactory sessionFactory)
        {
            this.SessionFactory = sessionFactory;
        }

        public ISessionFactory GetSessionFactory(string username, string databaseName)
        {
            using (var session = this.SessionFactory.OpenSession())
            {
                var userDatabase = session.Query<UserDatabase>()
                    .Where(x => x.Username == username 
                        && x.DatabaseName == databaseName)
                    .FirstOrDefault();
                
                if (userDatabase == null)
                {
                    throw new UserDatabaseNotFoundException(username, databaseName);
                }

                return CreateSessionFactory(userDatabase);
            }
        }

        private ISessionFactory CreateSessionFactory(UserDatabase userDatabase)
        {
            var nhConfiguration = Fluently.Configure();
            nhConfiguration.Database(PostgreSQLConfiguration.Standard
                .ConnectionString(c => {
                    c.Host(userDatabase.ConnectionHost);
                    c.Port(userDatabase.ConnectionPort);
                    c.Database(userDatabase.ConnectionDatabase);
                    c.Username(userDatabase.ConnectionUsername);
                    c.Password(userDatabase.ConnectionPassword);
                }));

            nhConfiguration.Mappings(m => m.FluentMappings.AddFromAssemblyOf<TableMapping>());

            return nhConfiguration.BuildSessionFactory();
        }
    }
}