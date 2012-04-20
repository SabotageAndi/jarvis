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
using Autofac;
using jarvis.client.common;
using jarvis.client.common.Actions.ActionCaller;
using jarvis.client.common.ServiceClients;

namespace jarvis.client.worker
{
    internal class Program
    {
        private static IContainer _container;


        private static void Bootstrap()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<CommonModule>();
            containerBuilder.RegisterType<ConfigFileConfiguration>().As<IConfiguration>().SingleInstance();
            containerBuilder.RegisterType<WorkerClient>().As<Client>().SingleInstance();
            containerBuilder.RegisterType<FileAction>().As<IFileAction>().SingleInstance();
            containerBuilder.RegisterType<WorkflowEngine>().As<IWorkflowEngine>().SingleInstance();

            _container = containerBuilder.Build();
        }


        private static void Main(string[] args)
        {
            Bootstrap();
            var client = _container.Resolve<Client>();


            client.Init(_container);
            client.Run();

            Console.ReadLine();
        }
    }

    public class WorkerClient : Client
    {
        private readonly IWorkflowEngine _workflowEngine;

        public WorkerClient(IClientService clientService, IConfiguration configuration, IServerStatusService serverStatusService,
                            IWorkflowEngine workflowEngine)
            : base(clientService, configuration, serverStatusService)
        {
            _workflowEngine = workflowEngine;
        }

        public override void Run()
        {
            base.Run();
            while (true)
            {
                if (!_workflowEngine.Do())
                {
                    Thread.Sleep(10000);
                }
            }
        }
    }
}