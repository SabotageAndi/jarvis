using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using jarvis.client.common.ServiceClients;

namespace jarvis.client.common
{
    public class CommonModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            builder.RegisterModule<ServiceClients.ServiceClientModule>();
            builder.RegisterType<Client>();
            builder.RegisterType<JarvisRestClient>().As<IJarvisRestClient>().InstancePerDependency();
        }
    }
}
