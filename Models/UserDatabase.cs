using System;

namespace PGManagerApi.Models
{
    public class UserDatabase
    {
        public virtual string Username { get; set; }
        public virtual string DatabaseName { get; set; }

        public virtual string ConnectionHost { get; set; }
        public virtual int ConnectionPort { get; set; }
        public virtual string ConnectionDatabase { get; set; }
        public virtual string ConnectionUsername { get; set; }
        public virtual string ConnectionPassword { get; set; }

        public override bool Equals(object obj)
        {
            if (obj is UserDatabase)
            {
                var compareTo = obj as UserDatabase;
                return this.Username == compareTo.Username
                    && this.DatabaseName == compareTo.DatabaseName;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(this.Username, this.DatabaseName);
        }
    }
}