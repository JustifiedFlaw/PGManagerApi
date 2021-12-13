using NHibernate;
using PGManagerApi.Models;

namespace PGManagerApi.Services
{
    public class UserService
    {
        private ISessionFactory SessionFactory;
        
        public UserService(ISessionFactory sessionFactory)
        {
            this.SessionFactory = sessionFactory;
        }

        public User Get(string name)
        {
            using (var session = this.SessionFactory.OpenSession())
            {
                 return session.Get<User>(name);
            }
        }

        public void Update(User user)
        {
            using (var session = this.SessionFactory.OpenSession())
            using (var transaction = session.BeginTransaction())
            {
                session.Merge(user);
                transaction.Commit();
            }
        }
    }
}