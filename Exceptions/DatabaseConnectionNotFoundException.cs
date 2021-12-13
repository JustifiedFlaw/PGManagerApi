using System;

namespace PGManagerApi.Exceptions
{
    public class DatabaseConnectionNotFoundException : Exception
    {
        public DatabaseConnectionNotFoundException(string username, string connectionName)
            : base($"Database connection {connectionName} of user {username} not found")
        {
        }
    }
}