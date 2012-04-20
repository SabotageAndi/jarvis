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

using System.Reflection;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.Wcf;
using jarvis.common.domain;
using jarvis.server.common.Database;
using jarvis.server.entities.Management;
using jarvis.server.web.Common.Database;
using jarvis.server.web.services;
using log4net.Config;

namespace jarvis.server.web
{
    public class Bootstrapper
    {
        public static IContainer Container;

        public static void init()
        {
            XmlConfigurator.Configure();

            var serverStatus = new ServerStatus();
            serverStatus.State = State.Instanciated;


            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterInstance(serverStatus).As<ServerStatus>().SingleInstance();

            containerBuilder.RegisterType<SessionFactory>().As<ISessionFactory>().SingleInstance();
            containerBuilder.RegisterType<TransactionProvider>().As<ITransactionProvider>().SingleInstance();


            containerBuilder.RegisterModule(new ServiceModule());
            containerBuilder.RegisterControllers(Assembly.GetExecutingAssembly());
            containerBuilder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            containerBuilder.RegisterModelBinderProvider();


            Container = containerBuilder.Build();

            AutofacHostFactory.Container = Container;
        }
    }
}