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

using Ninject;

namespace jarvis.server.common.Database
{
    public class TransactionProvider : ITransactionProvider
    {
        private static string _HttpContextCurrentScope = "NHibernate_Current_Scope";
        private readonly ISessionFactory _sessionFactory;
        private readonly IKernel _kernel;

        public TransactionProvider(ISessionFactory sessionFactory, IKernel kernel)
        {
            _sessionFactory = sessionFactory;
            _kernel = kernel;
        }

        public ITransactionScope CurrentScope
        {
            get { return (ITransactionScope) WebContext.Current.Items[_HttpContextCurrentScope]; }
        }

        public void SetCurrentScope(ITransactionScope transactionScope)
        {
            WebContext.Current.Items[_HttpContextCurrentScope] = transactionScope;
        }

        //TODO: Make real read-only transaction
        public ITransactionScope GetReadTransaction()
        {
            return new TransactionScope(_sessionFactory.OpenSession());
        }

        public ITransactionScope GetReadWriteTransaction()
        {
            return new TransactionScope(_sessionFactory.OpenSession());
        }

        public ITransactionScope StartReadTransaction()
        {
            var transaction = GetReadTransaction();
            transaction.BeginTransaction();
            return transaction;
        }

        public ITransactionScope StartReadWriteTransaction()
        {
            var transaction = GetReadWriteTransaction();
            transaction.BeginTransaction();
            return transaction;
        }
    }
}