using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Linq;
using jarvis.server.common.Database;
using jarvis.server.repositories;

namespace jarvis.addins.karma.server
{
    public class KarmaRepository : RepositoryBase<Karma>
    {
        public KarmaRepository(ITransactionProvider transactionProvider) : base(transactionProvider)
        {
        }

        public Karma GetByKey(string key)
        {
            return this.CurrentSession.Query<Karma>().Where(k => k.KarmaKey == key).SingleOrDefault();
        }
    }
}
