using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using jarvis.server.common.Database;
using jarvis.server.configuration;

namespace jarvis.server.repositories
{
    public interface IDatabaseManager
    {
        void UpdateSchema();
    }

    public class DatabaseManager : IDatabaseManager
    {
        private readonly INHibernateConfiguration _nHibernateConfiguration;

        public DatabaseManager(INHibernateConfiguration nHibernateConfiguration)
        {
            _nHibernateConfiguration = nHibernateConfiguration;
        }

        public void UpdateSchema()
        {
            var schemaUpdater = new SchemaUpdate(_nHibernateConfiguration.GetConfiguration().BuildConfiguration());

            schemaUpdater.Execute(true, true);
        }
    }
}
