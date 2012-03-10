using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;

namespace jarvis.server.repositories.Infrastructure
{
    public interface ITransactionFactory
    {
        ITransaction BeginTransaction(ISession session);
    }

    public class TransactionFactory : ITransactionFactory
    {
        public ITransaction BeginTransaction(ISession session)
        {
            return session.BeginTransaction();
        }
    }
}
