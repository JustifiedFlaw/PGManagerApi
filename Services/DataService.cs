using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Npgsql;
using NpgsqlTypes;
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

                command.CommandText = $"SELECT * FROM \"{table.SchemaName}\".\"{table.TableName}\" LIMIT {rowCount} OFFSET {startRow}";

                Data data;
                using (var reader = command.ExecuteReader())
                {
                    data = new Data(reader);
                    reader.Close();
                }

                return data;
            }
        }

        public void InsertData(string username, int connectionId, Table table, Data data)
        {
            using (var npgSqlConnection = this.DatabaseConnectionService.GetNpgsqlConnection(username, connectionId))
            {
                npgSqlConnection.Open();
                
                using (var transaction = npgSqlConnection.BeginTransaction())
                using (var command = new NpgsqlCommand())
                {
                    command.Connection = npgSqlConnection;

                    try
                    {
                        var rows = new List<string>();
                        for (var iRow = 0; iRow < data.Rows.Count; iRow++)
                        {
                            var row = data.Rows[iRow];

                            var fields = new List<string>();
                            foreach (var field in row)
                            {
                                var varName = $"{field.Key}{iRow}";

                                fields.Add("@" + varName);

                                var type = Enum.Parse<NpgsqlDbType>(data.FieldTypes[field.Key], true);
                                command.Parameters.Add(new NpgsqlParameter(varName, type)
                                {
                                    Value = field.Value
                                });
                            }

                            rows.Add("(" + string.Join(',', fields) + ")");
                        }

                        command.CommandText = $"INSERT INTO \"{table.SchemaName}\".\"{table.TableName}\" "
                                    + "(" + string.Join(',', data.FieldTypes.Select(f => f.Key)) + ") " 
                                    + "VALUES " + string.Join(',', rows);
                        
                        command.ExecuteNonQuery();


                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void UpdateData(string username, int connectionId, Table table, Update update)
        {
            using (var npgSqlConnection = this.DatabaseConnectionService.GetNpgsqlConnection(username, connectionId))
            {
                npgSqlConnection.Open();
                
                using (var command = new NpgsqlCommand())
                using (var transaction = npgSqlConnection.BeginTransaction())
                {
                    command.Connection = npgSqlConnection;

                    try
                    {
                        command.CommandText = $"UPDATE \"{table.SchemaName}\".\"{table.TableName}\" SET ";
                        command.CommandText += string.Join(',', update.Row.Select(f => $"{f.Key} = @{f.Key}"));
                        AddParameters(command, update.FieldTypes, update.Row);

                        command.CommandText += " WHERE ";
                        command.CommandText += string.Join(" AND ", update.Where.Select(f => $"{f.Key} = @{f.Key}"));
                        AddParameters(command, update.FieldTypes, update.Where);

                        command.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void DeleteData(string username, int connectionId, Table table, Delete delete)
        {
            using (var npgSqlConnection = this.DatabaseConnectionService.GetNpgsqlConnection(username, connectionId))
            {
                npgSqlConnection.Open();
                
                using (var command = new NpgsqlCommand())
                using (var transaction = npgSqlConnection.BeginTransaction())
                {
                    command.Connection = npgSqlConnection;

                    try
                    {
                        command.CommandText = $"DELETE FROM \"{table.SchemaName}\".\"{table.TableName}\" WHERE ";
                        command.CommandText += string.Join(" AND ", delete.Where.Select(f => $"{f.Key} = @{f.Key}"));
                        AddParameters(command, delete.FieldTypes, delete.Where);

                        command.ExecuteNonQuery();

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private static void AddParameters(NpgsqlCommand command, FieldTypes fieldTypes, Row row)
        {
            foreach (var field in row)
            {
                var type = Enum.Parse<NpgsqlDbType>(fieldTypes[field.Key], true);
                var parameter = new NpgsqlParameter($"@{field.Key}", type)
                {
                    Value = field.Value
                };
                command.Parameters.Add(parameter);
            }
        }
    }
}