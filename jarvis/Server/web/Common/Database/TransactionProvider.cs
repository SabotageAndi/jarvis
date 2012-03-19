using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NHibernate.Transaction;
using jarvis.server.common.Database;
using jarvis.server.repositories;

namespace jarvis.server.web.Common.Database
{
    public class TransactionProvider : ITransactionProvider
    {
        private readonly ISessionFactory _sessionFactory;
        private static string _HttpContextCurrentScope = "NHibernate_Current_Scope";

        public TransactionProvider(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public ITransactionScope CurrentScope
        {
            get { return (ITransactionScope) HttpContext.Current.Items[_HttpContextCurrentScope]; }
        }

        public void SetCurrentScope(ITransactionScope transactionScope)
        {
            HttpContext.Current.Items[_HttpContextCurrentScope] = transactionScope;
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
    }
}