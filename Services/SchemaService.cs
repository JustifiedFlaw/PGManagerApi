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

        public Table[] GetTables(string username, string connectionName)
        {
            var sessionFactory = this.DatabaseConnectionService.GetSessionFactory(username, connectionName);

            using (var session = sessionFactory.OpenSession())
            {
                 return session.Query<Table>().ToArray();
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