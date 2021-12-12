namespace PGManagerApi.Settings
{
    public class DatabaseSettings
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string Database { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public string ConnectionString
        {
            get
            {
                return new Npgsql.NpgsqlConnectionStringBuilder
                {
                    Host = this.Host,
                    Port = this.Port,
                    Database = this.Database,
                    Username = this.Username,
                    Password = this.Password
                }.ConnectionString;
            }
        }
    }
}