namespace PGManagerApi.Models
{
    public class User
    {
        public virtual string Name { get; set; }
        public virtual string PasswordHash { get; set; }

        public virtual string Password 
        {
            set 
            {
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(value);
            }
        }
    }
}