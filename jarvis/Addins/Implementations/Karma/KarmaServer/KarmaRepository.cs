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
        public Karma GetByKey(ITransactionScope transactionScope, string key)
        {
            return transactionScope.CurrentSession.Query<Karma>().Where(k => k.KarmaKey == key).SingleOrDefault();
        }
    }
}
