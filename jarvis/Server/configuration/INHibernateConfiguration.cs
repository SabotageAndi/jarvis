using System.Reflection;
using FluentNHibernate.Cfg;
using NHibernate;

namespace jarvis.server.configuration
{
    public interface INHibernateConfiguration
    {
        ISessionFactory GetSessionFactory();
        FluentConfiguration GetConfiguration();
        void AddAssembly(Assembly assembly);
        void RecreateSessionFactory();
    }

  
}