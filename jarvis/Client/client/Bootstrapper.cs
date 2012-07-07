using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using jarvis.client.common;
using jarvis.client.common.ServiceClients;
using log4net;
using log4net.Config;

namespace jarvis.client
{
    public class Bootstrapper
    {

        public static IKernel Container { get; private set; }

        public static void Init<ClientType>() where ClientType : Client
        {
            XmlConfigurator.Configure();
            var log = LogManager.GetLogger("worker-client");

            var containerBuilder = new StandardKernel();
            containerBuilder.Bind<Func<IKernel>>().ToMethod(ctx => () => Client.Container);
            containerBuilder.Load(new CommonModule(), new ClientModule(), new ServiceClientModule());
            containerBuilder.Bind<Client>().To<ClientType>().InSingletonScope();

            containerBuilder.Bind<ILog>().ToConstant(log).InSingletonScope();

            Container = containerBuilder;
        }
    }
}
