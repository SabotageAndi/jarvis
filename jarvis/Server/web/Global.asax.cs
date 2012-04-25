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
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using jarvis.common.domain;
using jarvis.server.common.Database;
using jarvis.server.entities.Management;
using jarvis.server.model.ActionHandling;
using jarvis.server.repositories;

namespace jarvis.server.web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected ServerStatus Status
        {
            get { return Bootstrapper.Container.Resolve<ServerStatus>(); }
        }

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
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
        }

        protected void Application_Start()
        {
            Bootstrapper.init();

            var databaseManager = Bootstrapper.Container.Resolve<IDatabaseManager>();
            databaseManager.UpdateSchema();

            AreaRegistration.RegisterAllAreas();

            DependencyResolver.SetResolver(new AutofacDependencyResolver(Bootstrapper.Container));


            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);

            LoadAddins();

            Status.State = State.Running;
        }

        private void LoadAddins()
        {
            var actionLogic = Bootstrapper.Container.Resolve<IActionLogic>();
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
            return Bootstrapper.Container.Resolve<ITransactionProvider>();
        }
    }
}