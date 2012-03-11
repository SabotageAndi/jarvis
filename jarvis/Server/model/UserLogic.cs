using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jarvis.server.entities;
using jarvis.server.repositories;
using jarvis.server.repositories.Infrastructure;

namespace jarvis.server.model
{
    public interface IUserLogic
    {
        User AddUser(string username, string password);
        User Login(string username, string password);
        void ChangePassword(User user, string oldPassword, string newPassword);
        User GetUser(string name);
    }

    public class UserLogic : IUserLogic
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ITransactionFactory _transactionFactory;
        private readonly IUserRepository _userRepository;

        public UserLogic(ISessionFactory sessionFactory, ITransactionFactory transactionFactory, IUserRepository userRepository)
        {
            _sessionFactory = sessionFactory;
            _transactionFactory = transactionFactory;
            _userRepository = userRepository;
        }

        public User AddUser(string username, string password)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = _transactionFactory.BeginTransaction(session))
                {
                    var user = _userRepository.Add(session, username, password);
                    transaction.Commit();

                    return user;
                }
            }
        }

        public User Login(string username, string password)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = _transactionFactory.BeginTransaction(session))
                {
                    return _userRepository.Login(session, username, password);
                }
            }
        }

        public void ChangePassword(User user, string oldPassword, string newPassword)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = _transactionFactory.BeginTransaction(session))
                {
                    if (user.Password == oldPassword)
                        _userRepository.ChangePassword(session, user, newPassword);
                }
            }
        }

        public User GetUser(string name)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = _transactionFactory.BeginTransaction(session))
                {
                    return _userRepository.GetUser(session, name);
                }
            }
        }
    }
}
