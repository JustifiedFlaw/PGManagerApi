using System.Collections.Generic;
using System.Linq;
using Npgsql;
using PGManagerApi.Models;

namespace PGManagerApi.Services
{
    public class DataService
    {
        DatabaseConnectionService DatabaseConnectionService;

        public DataService(DatabaseConnectionService databaseConnectionService)
        {
            this.DatabaseConnectionService = databaseConnectionService;
        }

        public Data GetData(string username, int connectionId, Table table, int startRow, int rowCount)
        {
            using (var npgSqlConnection = this.DatabaseConnectionService.GetNpgsqlConnection(username, connectionId))
            using (var command = new NpgsqlCommand())
            {
                npgSqlConnection.Open();
                command.Connection = npgSqlConnection;

                command.CommandText = $"SELECT * FROM \"{table.SchemaName}\".\"{table.TableName}\" LIMIT {rowCount}";

                Data data;
                using (var reader = command.ExecuteReader())
                {
                    data = new Data(reader);
                    reader.Close();
                }

                return data;
            }
        }
    }
}