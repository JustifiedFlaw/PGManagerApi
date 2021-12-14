using System;

namespace PGManagerApi.Exceptions
{
    public class DatabaseConnectionNotFoundException : Exception
    {
        public DatabaseConnectionNotFoundException(string username, int connectionId)
            : base($"Database connection {connectionId} of user {username} not found")
        {
        }
    }
}