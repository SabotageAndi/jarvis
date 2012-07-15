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
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Description;
using Funq;
using Ninject;
using ServiceStack.ServiceInterface;
using ServiceStack.WebHost.Endpoints;
using jarvis.client.common;
using jarvis.common.dtos.Requests;
using jarvis.common.logic;
using log4net;

namespace jarvis.addins.actions
{
    public interface IActionServiceHost
    {
        void Start();
        void Init();
    }

    public class ActionServiceHost : AppHostHttpListenerBase, IActionServiceHost
    {
        private readonly IKernel _kernel;
        private readonly IConfiguration _configuration;
        private readonly ILog _log;

        public ActionServiceHost(IKernel kernel, IConfiguration configuration, ILog log)
            :base("JarvisWorker", Assembly.GetExecutingAssembly())
        {
            _kernel = kernel;
            _configuration = configuration;
            _log = log;
        }

        private String GetBaseAddress()
        {
            return String.Format("http://{0}:{1}/", "*", _configuration.LocalPort);
        }

        public override void Configure(Container container)
        {
            container.Adapter = new NinjectIocAdapter(_kernel);

            base.SetConfig(new EndpointHostConfig
            {
                GlobalResponseHeaders =
                {
                    { "Access-Control-Allow-Origin", "*" },
                    { "Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS" },
                },
                DebugMode = true,
                WsdlServiceNamespace = "http://www.servicestack.net/types",
            });


            Routes.Add<ActionExecuteRequest>("/action/execute", "POST");
        }

        public void Start()
        {
            var baseAddress = GetBaseAddress();

            _log.InfoFormat("Starting ActionServiceHost on {0}", baseAddress);

            this.Start(baseAddress);
        }
    }
}