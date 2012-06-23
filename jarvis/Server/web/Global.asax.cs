// J.A.R.V.I.S. - Just A Rather Very Intelligent System
// Copyright (C) 2012 Andreas Willich <sabotageandi@gmail.com>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.ServiceModel;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using Ninject.Extensions.Wcf;
using Ninject.Web.Common;
using jarvis.common.domain;
using jarvis.server.common.Database;
using jarvis.server.configuration;
using jarvis.server.entities.Management;
using jarvis.server.model;
using jarvis.server.model.ActionHandling;
using jarvis.server.repositories;

namespace jarvis.server.web
{
    public class MvcApplication :  NinjectHttpApplication
    {
        protected ServerStatus Status
        {
            get { return Bootstrapper.Container.Get<ServerStatus>(); }
        }

        public void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new
                    {
                        controller = "Home",
                        action = "Index",
                        id = UrlParameter.Optional
                    } // Parameter defaults
                );
            /*
             *    Bind<TriggerService>().ToSelf().Named("jarvis.server.web.Services.TriggerService");
            Bind<EventHandlingService>().ToSelf().Named("jarvis.server.web.services.EventHandlingService");
            Bind<WorkflowService>().ToSelf().Named("jarvis.server.web.services.WorkflowService");
            Bind<ClientService>().ToSelf().Named("jarvis.server.web.services.ClientService");
            Bind<ServerStatusService>().ToSelf().Named("jarvis.server.web.services.ServerStatusService");
            Bind<ActionService>().ToSelf().Named("jarvis.server.web.services.ActionService");
             * */

            //routes.Add(new ServiceRoute("jarvis.server.web.services.ServerStatusService", new NinjectWebServiceHostFactory(), typeof(ServerStatusService)));
            //routes.Add(new ServiceRoute("jarvis.server.web.services.EventHandlingService", new NinjectWebServiceHostFactory(), typeof(EventHandlingService)));
            //routes.Add(new ServiceRoute("jarvis.server.web.services.WorkflowService", new NinjectWebServiceHostFactory(), typeof(WorkflowService)));
            //routes.Add(new ServiceRoute("jarvis.server.web.services.TriggerService", new NinjectWebServiceHostFactory(), typeof(TriggerService)));
            //routes.Add(new ServiceRoute("jarvis.server.web.services.ClientService", new NinjectWebServiceHostFactory(), typeof(ClientService)));
            //routes.Add(new ServiceRoute("jarvis.server.web.services.ActionService", new NinjectWebServiceHostFactory(), typeof(ActionService)));
        }

        protected override void OnApplicationStarted()
        {
            base.OnApplicationStarted();

            Bootstrapper.init();

            var databaseManager = Bootstrapper.Container.Get<IDatabaseManager>();
            databaseManager.UpdateSchema();

            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            //LoadAddins();

            Status.State = State.Running;
        }

      
        protected override IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => Bootstrapper.Container);
            //kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            kernel.Bind<ServiceHost>().To<NinjectServiceHost>();

            var serverStatus = new ServerStatus();
            serverStatus.State = State.Instanciated;


            kernel.Bind<ServerStatus>().ToConstant(serverStatus).InSingletonScope();

            kernel.Bind<ISessionFactory>().To<SessionFactory>().InSingletonScope();
            kernel.Bind<ITransactionProvider>().To<TransactionProvider>().InSingletonScope();

            kernel.Load(new RepositoryModule(), new ModelModule(), new ConfigurationModule());

            Bootstrapper.Container = kernel;
        
            return kernel;
        }

        private void LoadAddins()
        {
            var actionLogic = Bootstrapper.Container.Get<IActionLogic>();
            actionLogic.LoadServerActions();
        }

        public override void Init()
        {
            BeginRequest += OnBeginRequest;
            EndRequest += OnEndRequest;

            base.Init();
        }

        private void OnEndRequest(object sender, EventArgs eventArgs)
        {
            var transactionScope = GetTransactionProvider().CurrentScope;

            if (transactionScope != null)
            {
                transactionScope.Commit();
                transactionScope.Close();
            }
        }

        private void OnBeginRequest(object sender, System.EventArgs e)
        {
            var transactionProvider = GetTransactionProvider();
            transactionProvider.SetCurrentScope(transactionProvider.GetReadWriteTransaction());

            transactionProvider.CurrentScope.BeginTransaction();
        }

        private ITransactionProvider GetTransactionProvider()
        {
            return Bootstrapper.Container.Get<ITransactionProvider>();
        }
    }
}