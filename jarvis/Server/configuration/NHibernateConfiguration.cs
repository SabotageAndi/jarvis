using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using jarvis.server.entities;

namespace jarvis.server.configuration
{
    public class NHibernateConfiguration
    {
        public static ISessionFactory GetSessionFactory()
        {
            return GetConfiguration()
                .BuildSessionFactory();
        }

        public static FluentConfiguration GetConfiguration()
        {
            return Fluently.Configure()
                .Database(PostgreSQLConfiguration.Standard.ConnectionString(
                    builder => builder.Database("jarvis").Host("localhost").Username("jarvis").Password("jarvis").Port(5432)))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Workflow>());
        }
    }
}