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

using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using jarvis.server.common.Database;
using jarvis.server.entities;

namespace jarvis.server.repositories
{
    public interface IRepositoryBase<T> where T : Entity, new()
    {
        T Create();
        void Save(ITransactionScope transactionScope, T entity);
        void Refresh(ITransactionScope transactionScope, T entity);
        T GetById(ITransactionScope transactionScope, int id);
    }

    public class RepositoryBase<T> : IRepositoryBase<T> where T : Entity, new()
    {
        public virtual T Create()
        {
            return new T();
        }

        public virtual void Save(ITransactionScope transactionScope, T entity)
        {
            transactionScope.CurrentSession.SaveOrUpdate(entity);
        }

        public virtual void Refresh(ITransactionScope transactionScope, T entity)
        {
            transactionScope.CurrentSession.Refresh(entity);
        }

        public virtual T GetById(ITransactionScope transactionScope, int id)
        {
            return transactionScope.CurrentSession.Query<T>().Where(e => e.Id == id).SingleOrDefault();
        }

        public virtual void Delete(ITransactionScope transactionScope, T entity)
        {
            transactionScope.CurrentSession.Delete(entity);
        }

        public IEnumerable<T> GetAll(ITransactionScope transactionScope)
        {
            return transactionScope.CurrentSession.Query<T>();
        }
    }
}