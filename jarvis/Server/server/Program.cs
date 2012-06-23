using System;
using System.ServiceModel;
using Ninject;
using jarvis.common.domain;
using jarvis.server.common.Database;
using jarvis.server.configuration;
using jarvis.server.entities.Management;
using jarvis.server.model;
using jarvis.server.repositories;
using jarvis.server.services;

namespace jarvis.server
{
    class Program
    {
        static void Main(string[] args)
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => Bootstrapper.Container);

            var serverStatus = new ServerStatus();
            serverStatus.State = State.Instanciated;


            kernel.Bind<ServerStatus>().ToConstant(serverStatus).InSingletonScope();

            kernel.Bind<ISessionFactory>().To<SessionFactory>().InSingletonScope();
            kernel.Bind<ITransactionProvider>().To<TransactionProvider>().InSingletonScope();
            kernel.Bind<IServiceHostManager>().To<ServiceHostManager>().InSingletonScope();

            kernel.Load(new RepositoryModule(), new ModelModule(), new ConfigurationModule(), new ServiceModule());

            Bootstrapper.Container = kernel;
            Bootstrapper.init();

            var serviceHostManager = kernel.Get<IServiceHostManager>();
            serviceHostManager.Start();

            serverStatus.State = State.Running;


            Console.WriteLine("Server up and running ...");

            Console.ReadLine();
            serviceHostManager.Stop();
            Console.WriteLine("Server shutting down ...");
        }
    }
}
