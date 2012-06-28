using System;
using System.ServiceModel;
using Ninject;
using ServiceStack.CacheAccess;
using ServiceStack.CacheAccess.Providers;
using jarvis.common.domain;
using jarvis.server.common.Database;
using jarvis.server.configuration;
using jarvis.server.entities.Management;
using jarvis.server.model;
using jarvis.server.model.ActionHandling;
using jarvis.server.repositories;
using jarvis.server.services;
using log4net;
using ISessionFactory = jarvis.server.common.Database.ISessionFactory;

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

            var logger = LogManager.GetLogger("server");

            kernel.Bind<ILog>().ToConstant(logger).InSingletonScope();

            kernel.Bind<ServerStatus>().ToConstant(serverStatus).InSingletonScope();

            kernel.Bind<ISessionFactory>().To<SessionFactory>().InSingletonScope();
            kernel.Bind<ITransactionProvider>().To<TransactionProvider>().InSingletonScope();
            kernel.Bind<ICacheClient>().To<MemoryCacheClient>().InSingletonScope();

            kernel.Load(new RepositoryModule(), new ModelModule(), new ConfigurationModule());

            

            Bootstrapper.Container = kernel;
            Bootstrapper.init();

            var actionLogic = kernel.Get<IActionLogic>();
            actionLogic.LoadServerActions();


            var serviceAppHost = new ServiceAppHost(kernel);
            serviceAppHost.Init();
            serviceAppHost.Start("http://*:7778/");

            serverStatus.State = State.Running;


            Console.WriteLine("Server up and running ...");

            Console.ReadLine();
            serviceAppHost.Stop();
            Console.WriteLine("Server shutting down ...");
        }
    }
}
