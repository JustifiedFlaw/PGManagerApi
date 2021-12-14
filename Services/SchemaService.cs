using System;
using System.Collections.Generic;
using System.Linq;
using PGManagerApi.Models;

namespace PGManagerApi.Services
{
    public class SchemaService
    {
        DatabaseConnectionService DatabaseConnectionService;

        public SchemaService(DatabaseConnectionService databaseConnectionService)
        {
            this.DatabaseConnectionService = databaseConnectionService;
        }

        public Database[] GetDatabases(string username, string connectionName)
        {
            var sessionFactory = this.DatabaseConnectionService.GetSessionFactory(username, connectionName);

            using (var session = sessionFactory.OpenSession())
            {
                 return session.Query<Database>().ToArray();
            }
        }

        public void CreateDatabase(string username, string connectionName, Database database)
        {
            var sessionFactory = this.DatabaseConnectionService.GetSessionFactory(username, connectionName);

            using (var session = sessionFactory.OpenSession())
            {
                // TODO: protect against injections
                //       by parameter gave the error: syntax error at or near "$1"
                 session.CreateSQLQuery($"CREATE DATABASE {database.Name}")
                    .ExecuteUpdate();
            }
        }

        public Table[] GetTables(string username, string connectionName)
        {
            var sessionFactory = this.DatabaseConnectionService.GetSessionFactory(username, connectionName);

            using (var session = sessionFactory.OpenSession())
            {
                 return session.Query<Table>().ToArray();
            }
        }

        public void CreateTable(string username, string connectionName, Table table)
        {
            var sessionFactory = this.DatabaseConnectionService.GetSessionFactory(username, connectionName);

            using (var session = sessionFactory.OpenSession())
            {
                // TODO: protect against injections
                //       by parameter gave the error: syntax error at or near "$1"
                // TODO: columns parameter
                session.CreateSQLQuery($"CREATE TABLE \"{table.SchemaName}\".\"{table.TableName}\" (id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY)")
                    .ExecuteUpdate();
            }
        }

        public void DropTable(string username, string connectionName, Table table)
        {
            var sessionFactory = this.DatabaseConnectionService.GetSessionFactory(username, connectionName);

            using (var session = sessionFactory.OpenSession())
            {
                // TODO: protect against injections
                //       by parameter gave the error: syntax error at or near "$1"
                // TODO: columns parameter
                session.CreateSQLQuery($"DROP TABLE \"{table.SchemaName}\".\"{table.TableName}\"")
                    .ExecuteUpdate();
            }
        }

        public IEnumerable<Column> GetColumns(string username, string connectionName, Table table)
        {
            var sessionFactory = this.DatabaseConnectionService.GetSessionFactory(username, connectionName);

            using (var session = sessionFactory.OpenSession())
            {
                 return session.Query<Column>()
                    .Where(c => c.SchemaName == table.SchemaName
                        && c.TableName == table.TableName)
                    .OrderBy(c => c.OrdinalPosition)
                    .ToArray();
            }
        }
    }
}