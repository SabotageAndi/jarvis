using NHibernate;
using NHibernate.Tool.hbm2ddl;
using jarvis.server.configuration;

namespace jarvis.tools.initDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            var configuration = NHibernateConfiguration.GetConfiguration();
            var sessionFactory = configuration.BuildSessionFactory();

            using(ISession session = sessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    new SchemaExport(configuration.BuildConfiguration()).Create(false,true);
                }
            }

        }
    }
}
