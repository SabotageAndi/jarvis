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
using System.Threading;
using Ninject;
using jarvis.addins.actions;
using jarvis.client.common;
using jarvis.client.common.ServiceClients;
using log4net;
using log4net.Config;

namespace jarvis.client.worker
{
    internal class Program
    {
        public static IKernel _container;


        private static void Bootstrap()
        {
            XmlConfigurator.Configure();
            var log = LogManager.GetLogger("worker-client");

            var containerBuilder = new StandardKernel();
            containerBuilder.Bind<Func<IKernel>>().ToMethod(ctx => () => Client.Container);
            containerBuilder.Load(new CommonModule(), new ClientModule(), new ServiceClientModule());
            containerBuilder.Bind<Client>().To<WorkerClient>().InSingletonScope();
            containerBuilder.Bind<IWorkflowEngine>().To<WorkflowEngine>().InSingletonScope();

            containerBuilder.Bind<ILog>().ToConstant(log).InSingletonScope();

            _container = containerBuilder;
        }


        private static void Main(string[] args)
        {
            Thread.Sleep(10000);

            Bootstrap();
            var client = _container.Get<Client>();


            client.Init(_container);
            client.Run();

            Console.ReadLine();
        }
    }
}