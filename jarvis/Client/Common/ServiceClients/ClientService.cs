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

using RestSharp;
using jarvis.common.dtos.Management;

namespace jarvis.client.common.ServiceClients
{
    public interface IClientService
    {
        ClientDto Register(ClientDto clientDto);
        void Logon(ClientDto clientDto);
        void Logoff(ClientDto clientDto);
    }

    public class ClientService : ServiceBase, IClientService
    {
        public ClientService(IJarvisRestClient jarvisRestClient, IConfiguration configuration) : base(jarvisRestClient, configuration)
        {
        }

        protected override string ServiceName
        {
            get { return _configuration.ClientService; }
        }

        public ClientDto Register(ClientDto clientDto)
        {
            var registerRequest = JarvisRestClient.CreateRequest("client", Method.POST);
            registerRequest.AddBody(clientDto);

            return _jarvisRestClient.Execute<ClientDto>(registerRequest);
        }

        public void Logon(ClientDto clientDto)
        {
            var logonRequest = JarvisRestClient.CreateRequest("client/logon", Method.POST);
            logonRequest.AddBody(clientDto);

            _jarvisRestClient.Execute(logonRequest);
        }

        public void Logoff(ClientDto clientDto)
        {
            var logonRequest = JarvisRestClient.CreateRequest("client/logoff", Method.POST);
            logonRequest.AddBody(clientDto);

            _jarvisRestClient.Execute(logonRequest);
        }
    }
}