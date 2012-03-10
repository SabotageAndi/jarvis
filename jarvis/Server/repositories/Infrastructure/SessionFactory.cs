using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using jarvis.server.configuration;

namespace jarvis.server.repositories
{
    public interface ISessionFactory
    {
        ISession OpenSession();
    }

    public class SessionFactory : ISessionFactory
    {
        private readonly INHibernateConfiguration _nHibernateConfiguration;

        public SessionFactory(INHibernateConfiguration nHibernateConfiguration)
        {
            _nHibernateConfiguration = nHibernateConfiguration;
        }

        public ISession OpenSession()
        {
            return _nHibernateConfiguration.GetSessionFactory().OpenSession();
        }
    }
}
