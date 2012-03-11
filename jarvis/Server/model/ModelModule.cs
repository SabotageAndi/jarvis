using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using jarvis.server.repositories;

namespace jarvis.server.model
{
    public class ModelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new RepositoryModule());

            builder.RegisterType<TriggerLogic>().As<ITriggerLogic>().InstancePerDependency();
            builder.RegisterType<UserLogic>().As<IUserLogic>().InstancePerDependency();
            base.Load(builder);
        }
    }
}
