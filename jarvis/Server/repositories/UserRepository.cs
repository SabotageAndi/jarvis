using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Linq;
using jarvis.server.entities;

namespace jarvis.server.repositories
{
    public interface IUserRepository
    {
        User Login(ISession session, string username, string password);
        User Add(ISession session, string username, string password);
        void ChangePassword(ISession session, User user, string newPassword);
        User GetUser(ISession session, string name);
    }

    public class UserRepository : IUserRepository
    {
        public User Login(ISession session, string username, string password)
        {
            return session.Query<User>().Where(u => u.Username == username && u.Password == password).SingleOrDefault();
        }

        public User Add(ISession session, string username, string password)
        {
            var user = new User()
                       {
                           Username = username,
                           Password = password
                       };

            session.SaveOrUpdate(user);

            return user;
        }

        public void ChangePassword(ISession session, User user, string newPassword)
        {
            user.Password = newPassword;
            session.Update(user);
        }

        public User GetUser(ISession session, string name)
        {
            return session.Query<User>().Where(u => u.Username == name).SingleOrDefault();
        }
    }
}
