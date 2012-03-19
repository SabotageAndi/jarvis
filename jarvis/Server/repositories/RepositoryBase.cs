using NHibernate;
using jarvis.server.common.Database;

namespace jarvis.server.repositories
{
    public class RepositoryBase
    {
        private ITransactionProvider _transactionProvider;

        public RepositoryBase(ITransactionProvider transactionProvider)
        {
            _transactionProvider = transactionProvider;
        }

        protected ISession CurrentSession
        {
            get { return _transactionProvider.CurrentScope.CurrentSession; }
        }
    }
}