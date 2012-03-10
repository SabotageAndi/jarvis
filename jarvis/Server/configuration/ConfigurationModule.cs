using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using NHibernate;

namespace jarvis.server.configuration
{
    public class ConfigurationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PostgreSqlConfiguration>().As<INHibernateConfiguration>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
