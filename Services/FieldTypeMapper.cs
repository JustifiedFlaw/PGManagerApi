using System;
using NpgsqlTypes;

namespace PGManagerApi.Services
{
    public static class FieldTypeMapper
    {
        public static NpgsqlDbType Map(string schemaName)
        {
            switch (schemaName.ToLower())
            {
                case "bit varying":
                    return NpgsqlDbType.Bit;
                case "double precision":
                    return NpgsqlDbType.Double;
                case "character varying":
                    return NpgsqlDbType.Varchar;
                case "time without time zone":
                    return NpgsqlDbType.Time;
                case "time with time zone":
                    return NpgsqlDbType.TimeTz;
                case "timestamp without time zone":
                     return NpgsqlDbType.Timestamp;
                case "timestamp with time zone":
                    return NpgsqlDbType.TimestampTz;
                default:
                    return Enum.Parse<NpgsqlDbType>(schemaName, true);
            }
        }
    }
}