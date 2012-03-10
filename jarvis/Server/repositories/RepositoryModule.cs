using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using jarvis.server.configuration;
using jarvis.server.repositories.Infrastructure;

namespace jarvis.server.repositories
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new ConfigurationModule());

            builder.RegisterType<TransactionFactory>().As<ITransactionFactory>().InstancePerLifetimeScope();
            builder.RegisterType<SessionFactory>().As<ISessionFactory>().InstancePerLifetimeScope();

            builder.RegisterType<TriggerRepository>().As<ITriggerRepository>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}
