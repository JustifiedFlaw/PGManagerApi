using System;
using System.Linq;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using PGManagerApi.Exceptions;
using PGManagerApi.Mappings;
using PGManagerApi.Models;

namespace PGManagerApi.Services
{
    public class DatabaseConnectionService
    {
        ISessionFactory SessionFactory;

        public DatabaseConnectionService(ISessionFactory sessionFactory)
        {
            this.SessionFactory = sessionFactory;
        }

        public DatabaseConnection[] GetConnections(string username)
        {
            using (var session = this.SessionFactory.OpenSession())
            {
                return session.Query<DatabaseConnection>()
                    .Where(x => x.Username == username)
                    .ToArray();
            }
        }

        public DatabaseConnection GetConnection(string username, string connectionName)
        {
            using (var session = this.SessionFactory.OpenSession())
            {
                var connection = session.Query<DatabaseConnection>()
                    .Where(x => x.Username == username 
                        && x.ConnectionName == connectionName)
                    .FirstOrDefault();

                if (connection == null)
                {
                    throw new DatabaseConnectionNotFoundException(username, connectionName);
                }

                return connection;
            }
        }

        public ISessionFactory GetSessionFactory(string username, string connectionName)
        {
            var connection = this.GetConnection(username, connectionName);

            return CreateSessionFactory(connection);
        }

        public void Update(DatabaseConnection connection)
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Merge(connection);
                transaction.Commit();
            }
        }

        public void Add(DatabaseConnection connection)
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                connection.Id = 0;
                session.SaveOrUpdate(connection);
                transaction.Commit();
            }
        }

        private ISessionFactory CreateSessionFactory(DatabaseConnection databaseConnection)
        {
            var nhConfiguration = Fluently.Configure();
            nhConfiguration.Database(PostgreSQLConfiguration.Standard
                .ConnectionString(c => {
                    c.Host(databaseConnection.ConnectionHost);
                    c.Port(databaseConnection.ConnectionPort);
                    c.Database(databaseConnection.ConnectionDatabase);
                    c.Username(databaseConnection.ConnectionUsername);
                    c.Password(databaseConnection.ConnectionPassword);
                }));

            nhConfiguration.Mappings(m => m.FluentMappings.AddFromAssemblyOf<TableMapping>());

            return nhConfiguration.BuildSessionFactory();
        }
    }
}