using System;

namespace PGManagerApi.Exceptions
{
    public class UserDatabaseNotFoundException : Exception
    {
        public UserDatabaseNotFoundException(string username, string databaseName)
            : base($"Database {databaseName} of user {username} not found")
        {
        }
    }
}