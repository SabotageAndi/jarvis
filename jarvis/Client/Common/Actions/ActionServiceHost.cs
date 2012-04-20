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
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace jarvis.client.common.Actions
{
    public interface IActionServiceHost
    {
        void Start();
    }

    public class ActionServiceHost : IActionServiceHost
    {
        private readonly IActionService _actionService;
        private readonly IConfiguration _configuration;
        private ServiceHost _serviceHost;

        public ActionServiceHost(IActionService actionService, IConfiguration configuration)
        {
            _actionService = actionService;
            _configuration = configuration;
        }

        public void Start()
        {
            var baseAddress = GetBaseAddress();

            var contractDescription = ContractDescription.GetContract(typeof (ActionService));
            var restBinding = new WebHttpBinding(WebHttpSecurityMode.None);
            var endpointAddress = new EndpointAddress(baseAddress + "action");

            var restEndPoint = new ServiceEndpoint(contractDescription, restBinding, endpointAddress);

            restEndPoint.Behaviors.Add(new WebHttpBehavior());

            _serviceHost = new ServiceHost(_actionService, new Uri(baseAddress));
            _serviceHost.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.None;
            _serviceHost.AddServiceEndpoint(restEndPoint);
            _serviceHost.Open();
        }

        private String GetBaseAddress()
        {
            return String.Format("http://{0}:{1}/", "localhost", _configuration.LocalPort);
        }
    }
}
