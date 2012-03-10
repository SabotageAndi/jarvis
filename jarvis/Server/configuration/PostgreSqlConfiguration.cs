using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using jarvis.server.entities;

namespace jarvis.server.configuration
{
    public interface INHibernateConfiguration
    {
        ISessionFactory GetSessionFactory();
        FluentConfiguration GetConfiguration();
    }

    public class PostgreSqlConfiguration : INHibernateConfiguration
    {
        private FluentConfiguration _configuration;

        public ISessionFactory GetSessionFactory()
        {
           return GetConfiguration().BuildSessionFactory();
        }

        public FluentConfiguration GetConfiguration()
        {
            if (_configuration != null) 
                return _configuration;

            _configuration = Fluently.Configure()
                .Database(PostgreSQLConfiguration.Standard.ConnectionString(
                    builder => builder.Database("jarvis").Host("localhost").Username("jarvis").Password("jarvis").Port(5432)))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Workflow>());

            return _configuration;
        }
    }
}