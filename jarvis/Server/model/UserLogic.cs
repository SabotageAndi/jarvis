// J.A.R.V.I.S. - Just a really versatile intelligent system
// Copyright (C) 2012 Andreas Willich <sabotageandi@gmail.com>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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

        public UserLogic(ISessionFactory sessionFactory, ITransactionFactory transactionFactory,
                         IUserRepository userRepository)
        {
            _sessionFactory = sessionFactory;
            _transactionFactory = transactionFactory;
            _userRepository = userRepository;
        }

        #region IUserLogic Members

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

        #endregion
    }
}