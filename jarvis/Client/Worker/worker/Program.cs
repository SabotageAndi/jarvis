﻿// J.A.R.V.I.S. - Just A Rather Very Intelligent System
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


        private static void Main(string[] args)
        {
            Thread.Sleep(10000);

            Bootstrapper.Init<WorkerClient>("worker-client");

            Bootstrapper.Container.Bind<IWorkflowEngine>().To<WorkflowEngine>().InSingletonScope();
            Bootstrapper.Container.Bind<IWorkerLockManager>().To<WorkerLockManager>().InSingletonScope();
            Bootstrapper.Container.Bind<IWorkerServiceHost>().To<WorkerServiceHost>().InSingletonScope();

            var client = Bootstrapper.Container.Get<Client>();


            client.Init(Bootstrapper.Container);
            client.Run();
        }
    }
}