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
        SchemaService SchemaService;

        public DataService(DatabaseConnectionService databaseConnectionService, SchemaService schemaService)
        {
            this.DatabaseConnectionService = databaseConnectionService;
            this.SchemaService = schemaService;
        }

        public Data GetData(string username, int connectionId, Table table, Row where, int startRow, int rowCount)
        {
            using (var npgSqlConnection = this.DatabaseConnectionService.GetNpgsqlConnection(username, connectionId))
            using (var command = new NpgsqlCommand())
            {
                npgSqlConnection.Open();
                command.Connection = npgSqlConnection;

                command.CommandText = $"SELECT * FROM \"{table.SchemaName}\".\"{table.TableName}\"";

                if (where != null && where.Count > 0)
                {
                    var fieldTypes = GetFieldTypes(username, connectionId, table);

                    command.CommandText += " WHERE ";
                    command.CommandText += string.Join(" AND ", where.Select(f => $"\"{f.Key}\" = @{f.Key}"));
                    AddParameters(command, fieldTypes, where);
                }

                command.CommandText += $" LIMIT {rowCount} OFFSET {startRow}";

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

                    var fieldTypes = GetFieldTypes(username, connectionId, table);

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

                                var type = fieldTypes[field.Key];
                                command.Parameters.Add(new NpgsqlParameter(varName, type)
                                {
                                    Value = Parse(field.Value, type)
                                });
                            }

                            rows.Add("(" + string.Join(',', fields) + ")");
                        }

                        command.CommandText = $"INSERT INTO \"{table.SchemaName}\".\"{table.TableName}\" "
                                    + "(" + string.Join(',', data.Rows.First().Select(f => "\"" + f.Key + "\"")) + ") " 
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
                        var fieldTypes = GetFieldTypes(username, connectionId, table);

                        command.CommandText = $"UPDATE \"{table.SchemaName}\".\"{table.TableName}\" SET ";
                        command.CommandText += string.Join(',', update.Row.Select(f => $"\"{f.Key}\" = @{f.Key}"));
                        AddParameters(command, fieldTypes, update.Row);

                        command.CommandText += " WHERE ";
                        command.CommandText += string.Join(" AND ", update.Where.Select(f => $"\"{f.Key}\" = @{f.Key}"));
                        AddParameters(command, fieldTypes, update.Where);

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

        public void DeleteData(string username, int connectionId, Table table, Row where)
        {
            if (where == null || where.Count == 0)
            {
                throw new ArgumentException("No fields for where of delete provided");
            }

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
                        command.CommandText += string.Join(" AND ", where.Select(f => $"\"{f.Key}\" = @{f.Key}"));
                        
                        var fieldTypes = GetFieldTypes(username, connectionId, table);

                        AddParameters(command, fieldTypes, where);

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
                var type = fieldTypes[field.Key];
                var parameter = new NpgsqlParameter($"@{field.Key}", type)
                {
                    Value = Parse(field.Value, type)
                };
                command.Parameters.Add(parameter);
            }
        }

        private static object Parse(object value, NpgsqlDbType type)
        {
            var stringValue = value?.ToString();
            switch (type)
            {
                case NpgsqlDbType.Bigint:
                case NpgsqlDbType.Double:
                case NpgsqlDbType.Integer:
                case NpgsqlDbType.Numeric:
                case NpgsqlDbType.Real:
                case NpgsqlDbType.Smallint:
                    if (string.IsNullOrWhiteSpace(stringValue))
                    {
                        return DBNull.Value;
                    }
                    return double.Parse(stringValue);

                case NpgsqlDbType.Time:
                case NpgsqlDbType.TimeTz:
                case NpgsqlDbType.Timestamp:
                case NpgsqlDbType.TimestampTz:
                case NpgsqlDbType.Date:
                    if (string.IsNullOrWhiteSpace(stringValue))
                    {
                        return DBNull.Value;
                    }
                    return DateTime.Parse(stringValue);

                default:
                    if (stringValue == null)
                    {
                        return DBNull.Value;
                    }
                    return stringValue;
            }
        }

        private FieldTypes GetFieldTypes(string username, int connectionId, Table table)
        {
            var columns = this.SchemaService.GetColumns(username, connectionId, table);

            var fieldTypes = new FieldTypes();
            foreach (var column in columns)
            {
                fieldTypes.Add(column.ColumnName, FieldTypeMapper.Map(column.DataType));                
            }

            return fieldTypes;
        }
    }
}