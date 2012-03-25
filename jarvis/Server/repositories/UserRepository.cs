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

using System.Linq;
using NHibernate.Linq;
using jarvis.server.common.Database;
using jarvis.server.entities;
using jarvis.server.entities.Management;

namespace jarvis.server.repositories
{
    public interface IUserRepository
    {
        User Login(string username, string password);
        User Add(string username, string password);
        void ChangePassword(User user, string newPassword);
        User GetUser(string name);
    }

    public class UserRepository : RepositoryBase, IUserRepository
    {
        public UserRepository(ITransactionProvider transactionProvider) : base(transactionProvider)
        {
        }

        public User Login(string username, string password)
        {
            return CurrentSession.Query<User>().Where(u => u.Username == username && u.Password == password).SingleOrDefault();
        }

        public User Add(string username, string password)
        {
            var user = new User()
                           {
                               Username = username,
                               Password = password
                           };

            CurrentSession.SaveOrUpdate(user);

            return user;
        }

        public void ChangePassword(User user, string newPassword)
        {
            user.Password = newPassword;
            CurrentSession.SaveOrUpdate(user);
        }

        public User GetUser(string name)
        {
            return CurrentSession.Query<User>().Where(u => u.Username == name).SingleOrDefault();
        }
    }
}