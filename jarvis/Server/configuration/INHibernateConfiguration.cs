using FluentNHibernate.Cfg;
using NHibernate;

namespace jarvis.server.configuration
{
    public interface INHibernateConfiguration
    {
        ISessionFactory GetSessionFactory();
        FluentConfiguration GetConfiguration();
    }

  
}