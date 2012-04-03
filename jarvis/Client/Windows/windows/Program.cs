using System.Threading;
using Autofac;
using Autofac.Core;
using jarvis.client.common;
using jarvis.client.common.ServiceClients;

namespace jarvis.client.windows
{
    class Program
    {
        private static IContainer _container;

        static void Main(string[] args)
        {
            Bootstrap();

            var client = _container.Resolve<Client>();

            //TODO check if server online
            Thread.Sleep(30000);

            client.Init();

            client.Run();
        }

        static void Bootstrap()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<CommonModule>();
            containerBuilder.RegisterType<ConfigFileConfiguration>().As<IConfiguration>().SingleInstance();

            _container = containerBuilder.Build();
        }
    }
}
