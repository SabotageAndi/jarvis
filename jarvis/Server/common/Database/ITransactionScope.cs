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

using System;
using NHibernate;

namespace jarvis.server.common.Database
{
    public interface ITransactionScope : IDisposable
    {
        ISession CurrentSession { get; }
        void BeginTransaction();
        void Commit();
        void Rollback();
        void Close();
        void Flush();
    }

    public class TransactionScope : ITransactionScope
    {
        private readonly ISession _session;
        private ITransaction _transaction;

        public TransactionScope(ISession session)
        {
            _session = session;
        }

        public void Dispose()
        {
            if (_transaction != null)
            {
                _transaction.Dispose();
            }

            _session.Close();

            if (_session != null)
            {
                _session.Dispose();
            }
        }

        public ISession CurrentSession
        {
            get { return _session; }
        }

        public void BeginTransaction()
        {
            _transaction = _session.BeginTransaction();
        }

        public void Commit()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
            }
        }

        public void Rollback()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
            }
        }

        public void Close()
        {
            if (_session != null)
            {
                _session.Close();
            }
        }

        public void Flush()
        {
            if (_session != null)
            {
                _session.Flush();
            }
        }
    }
}