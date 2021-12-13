using System;
using System.Collections.Generic;
using System.Linq;
using PGManagerApi.Models;

namespace PGManagerApi.Services
{
    public class SchemaService
    {
        UserDatabaseService UserDatabaseService;

        public SchemaService(UserDatabaseService userDatabaseService)
        {
            this.UserDatabaseService = userDatabaseService;
        }

        public Table[] GetTables(string username, string databaseName)
        {
            var sessionFactory = this.UserDatabaseService.GetSessionFactory(username, databaseName);

            using (var session = sessionFactory.OpenSession())
            {
                 return session.Query<Table>().ToArray();
            }
        }

        public IEnumerable<Column> GetColumns(string username, string databaseName, Table table)
        {
            var sessionFactory = this.UserDatabaseService.GetSessionFactory(username, databaseName);

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