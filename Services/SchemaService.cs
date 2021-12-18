using System.Collections.Generic;
using System.Linq;
using Npgsql;
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

        public Database[] GetDatabases(string username, int connectionId)
        {
            var sessionFactory = this.DatabaseConnectionService.GetSessionFactory(username, connectionId);

            using (var session = sessionFactory.OpenSession())
            {
                 return session.Query<Database>().ToArray();
            }
        }

        public void CreateDatabase(string username, int connectionId, Database database)
        {
            var sessionFactory = this.DatabaseConnectionService.GetSessionFactory(username, connectionId);

            using (var session = sessionFactory.OpenSession())
            {
                 session.CreateSQLQuery($"CREATE DATABASE {database.Name}")
                    .ExecuteUpdate();
            }
        }

        public Table[] GetTables(string username, int connectionId)
        {
            var sessionFactory = this.DatabaseConnectionService.GetSessionFactory(username, connectionId);

            using (var session = sessionFactory.OpenSession())
            {
                 return session.Query<Table>().ToArray();
            }
        }

        public void CreateTable(string username, int connectionId, Table table)
        {
            var sessionFactory = this.DatabaseConnectionService.GetSessionFactory(username, connectionId);

            using (var session = sessionFactory.OpenSession())
            {
                // TODO: protect against injections
                //       by parameter gave the error: syntax error at or near "$1"
                // TODO: columns parameter
                session.CreateSQLQuery($"CREATE TABLE \"{table.SchemaName}\".\"{table.TableName}\" (id BIGINT PRIMARY KEY GENERATED ALWAYS AS IDENTITY)")
                    .ExecuteUpdate();
            }
        }

        public void RenameTable(string username, int connectionId, Table table, string newName)
        {
            var sessionFactory = this.DatabaseConnectionService.GetSessionFactory(username, connectionId);

            using (var session = sessionFactory.OpenSession())
            {
                session.CreateSQLQuery($"ALTER TABLE \"{table.SchemaName}\".\"{table.TableName}\"" +
                                    $"RENAME TO \"{newName}\"")
                    .ExecuteUpdate();
            }
        }

        public void DropTable(string username, int connectionId, Table table)
        {
            var sessionFactory = this.DatabaseConnectionService.GetSessionFactory(username, connectionId);

            using (var session = sessionFactory.OpenSession())
            {
                session.CreateSQLQuery($"DROP TABLE \"{table.SchemaName}\".\"{table.TableName}\"")
                    .ExecuteUpdate();
            }
        }

        public IEnumerable<Column> GetColumns(string username, int connectionId, Table table)
        {
            var sessionFactory = this.DatabaseConnectionService.GetSessionFactory(username, connectionId);

            using (var session = sessionFactory.OpenSession())
            {
                 return session.Query<Column>()
                    .Where(c => c.SchemaName == table.SchemaName
                        && c.TableName == table.TableName)
                    .OrderBy(c => c.OrdinalPosition)
                    .ToArray();
            }
        }

        public void AddColumns(string username, int connectionId, Table table, IEnumerable<Column> columns)
        {
            if (columns.Count() > 0)
            {
                using (var npgSqlConnection = this.DatabaseConnectionService.GetNpgsqlConnection(username, connectionId))
                using (var command = new NpgsqlCommand())
                {
                    npgSqlConnection.Open();
                    command.Connection = npgSqlConnection;

                    var query = $"ALTER TABLE \"{table.SchemaName}\".\"{table.TableName}\"";

                    query += string.Join(',',
                        columns.Select(c => $"\nADD COLUMN {c.ColumnName} {c.GetTypeString()}"));

                    command.CommandText = query;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void RenameColumn(string username, int connectionId, Table table, string columnName, string newName)
        {
            var sessionFactory = this.DatabaseConnectionService.GetSessionFactory(username, connectionId);

            using (var session = sessionFactory.OpenSession())
            {
                session.CreateSQLQuery($"ALTER TABLE \"{table.SchemaName}\".\"{table.TableName}\"" + 
                                    $"\nRENAME COLUMN {columnName} TO  {newName}")
                    .ExecuteUpdate();
            }
        }

        public void DropColumn(string username, int connectionId, Table table, string columnName)
        {
            var sessionFactory = this.DatabaseConnectionService.GetSessionFactory(username, connectionId);

            using (var session = sessionFactory.OpenSession())
            {
                session.CreateSQLQuery($"ALTER TABLE \"{table.SchemaName}\".\"{table.TableName}\"" + 
                                    $"\nDROP COLUMN {columnName}")
                    .ExecuteUpdate();
            }
        }

        public List<string> GetPrimaryKey(string username, int connectionId, Table table)
        {
            using (var npgSqlConnection = this.DatabaseConnectionService.GetNpgsqlConnection(username, connectionId))
            using (var command = new NpgsqlCommand())
            {
                npgSqlConnection.Open();
                command.Connection = npgSqlConnection;

                command.CommandText = 
                    "SELECT pg_attribute.attname " +
                    "FROM pg_index, pg_class, pg_attribute, pg_namespace " +
                    "WHERE pg_class.oid = @table::regclass " + 
                        "AND indrelid = pg_class.oid " +
                        "AND nspname = @schema " +
                        "AND pg_class.relnamespace = pg_namespace.oid " +
                        "AND pg_attribute.attrelid = pg_class.oid " +
                        "AND pg_attribute.attnum = any(pg_index.indkey) " +
                        "AND indisprimary";
                        
                command.Parameters.AddWithValue("table", table.TableName);
                command.Parameters.AddWithValue("schema", table.SchemaName);

                var fields = new List<string>();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                         fields.Add((string)reader[0]);
                    }
                    reader.Close();
                }

                return fields;
            }
        }
    }
}