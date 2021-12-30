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
                case "double precision":
                    return NpgsqlDbType.Double;
                case "character varying":
                    return NpgsqlDbType.Varchar;
                default:
                    return Enum.Parse<NpgsqlDbType>(schemaName, true);
            }
        }
    }
}