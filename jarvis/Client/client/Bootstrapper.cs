using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using jarvis.addins.actions;
using jarvis.client.common;
using jarvis.client.common.ServiceClients;
using log4net;
using log4net.Config;

namespace jarvis.client
{
    public class Bootstrapper
    {

        public static IKernel Container { get; private set; }

        public static void Init<ClientType>(string logName) where ClientType : Client
        {
            XmlConfigurator.Configure();
            var log = LogManager.GetLogger(logName);

            var containerBuilder = new StandardKernel();
            containerBuilder.Bind<Func<IKernel>>().ToMethod(ctx => () => Client.Container);
            containerBuilder.Load(new CommonModule(), new ClientModule(), new ServiceClientModule(), new ActionModule());
            containerBuilder.Bind<Client>().To<ClientType>().InSingletonScope();

            containerBuilder.Bind<ILog>().ToConstant(log).InSingletonScope();

            Container = containerBuilder;
        }
    }
}
