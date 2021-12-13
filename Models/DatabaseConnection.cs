using System;

namespace PGManagerApi.Models
{
    public class DatabaseConnection
    {
        public virtual string Username { get; set; }
        public virtual string ConnectionName { get; set; }

        public virtual string ConnectionHost { get; set; }
        public virtual int ConnectionPort { get; set; }
        public virtual string ConnectionDatabase { get; set; }
        public virtual string ConnectionUsername { get; set; }
        public virtual string ConnectionPassword { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is DatabaseConnection)
            {
                var compareTo = obj as DatabaseConnection;
                return this.Username == compareTo.Username
                    && this.ConnectionName == compareTo.ConnectionName;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Username, this.ConnectionName);
        }
    }
}