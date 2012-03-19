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
        #region IUserRepository Members

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

        #endregion
    }
}