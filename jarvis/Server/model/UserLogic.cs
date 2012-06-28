// J.A.R.V.I.S. - Just A Rather Very Intelligent System
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

using jarvis.server.common.Database;
using jarvis.server.entities.Management;
using jarvis.server.repositories;

namespace jarvis.server.model
{
    public interface IUserLogic
    {
        User AddUser(ITransactionScope transactionScope, string username, string password);
        User Login(ITransactionScope transactionScope, string username, string password);
        bool ChangePassword(ITransactionScope transactionScope, User user, string oldPassword, string newPassword);
        User GetUser(ITransactionScope transactionScope, string name);
    }

    public class UserLogic : IUserLogic
    {
        private readonly IUserRepository _userRepository;

        public UserLogic(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public User AddUser(ITransactionScope transactionScope, string username, string password)
        {
            var user = _userRepository.Add(transactionScope, username, password);
            return user;
        }

        public User Login(ITransactionScope transactionScope, string username, string password)
        {
            return _userRepository.Login(transactionScope, username, password);
        }

        public bool ChangePassword(ITransactionScope transactionScope, User user, string oldPassword, string newPassword)
        {
            if (user.Password == oldPassword)
            {
                _userRepository.ChangePassword(transactionScope, user, newPassword);
                return true;
            }

            return false;
        }

        public User GetUser(ITransactionScope transactionScope, string name)
        {
            return _userRepository.GetUser(transactionScope, name);
        }
    }
}