using System;

namespace PGManagerApi.Settings
{
    public static class DatabaseSettingsFactory
    {
        public static DatabaseSettings Load()
        {
            var databaseSettings = new DatabaseSettings();
            databaseSettings.Host = GetEnvironmentString("STORAGE_HOST");
            databaseSettings.Port = GetEnvironmentInteger("STORAGE_PORT");
            databaseSettings.Database = GetEnvironmentString("STORAGE_DB");
            databaseSettings.Username = GetEnvironmentString("STORAGE_USER");
            databaseSettings.Password = GetEnvironmentString("STORAGE_PWRD");

            return databaseSettings;
        }

        private static string GetEnvironmentString(string name)
        {
            var str = Environment.GetEnvironmentVariable(name);

            if (string.IsNullOrWhiteSpace(str))
            {
                throw new Exception($"Environment variable {name} is empty or missing");
            }

            return str;
        }

        private static int GetEnvironmentInteger(string name)
        {
            var str = Environment.GetEnvironmentVariable(name);
            if (int.TryParse(str, out int value))
            {
                return value;
            }

            throw new Exception($"Expected an integer in environment variable {name}, but was \"{str}\"");
        }
    }
}