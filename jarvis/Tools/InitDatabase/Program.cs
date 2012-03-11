﻿using NHibernate;
using NHibernate.Tool.hbm2ddl;
using jarvis.server.configuration;
using jarvis.server.entities;

namespace jarvis.tools.initDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = new PostgreSqlConfiguration().GetConfiguration();
            var sessionFactory = configuration.BuildSessionFactory();

            using (ISession session = sessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    new SchemaExport(configuration.BuildConfiguration()).Create(false, true);

                    var rootFolder = new Folder();
                    rootFolder.Name = "/";

                    session.SaveOrUpdate(rootFolder);

                    transaction.Commit();
                }



            }

        }
    }
}