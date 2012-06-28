using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using jarvis.server.common.Database;
using jarvis.server.configuration;
using log4net;

namespace jarvis.server.repositories
{
    public interface IDatabaseManager
    {
        void UpdateSchema();
    }

    public class DatabaseManager : IDatabaseManager
    {
        private readonly INHibernateConfiguration _nHibernateConfiguration;
        private readonly ILog _log;

        public DatabaseManager(INHibernateConfiguration nHibernateConfiguration, ILog log)
        {
            _nHibernateConfiguration = nHibernateConfiguration;
            _log = log;
        }

        public void UpdateSchema()
        {
            var schemaUpdater = new SchemaUpdate(_nHibernateConfiguration.GetConfiguration().BuildConfiguration());

            schemaUpdater.Execute(true, true);

            if (schemaUpdater.Exceptions.Any())
            {
                foreach (var exception in schemaUpdater.Exceptions)
                {
                    _log.ErrorFormat("Error at updating database schema: {0}", exception);
                }
            }
        }
    }
}
