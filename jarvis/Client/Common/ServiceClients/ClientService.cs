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

using jarvis.common.dtos;
using jarvis.common.dtos.Management;
using jarvis.common.dtos.Requests;

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
        public ClientService(IJarvisRestClient jarvisRestClient, IConfiguration configuration)
            : base(jarvisRestClient, configuration)
        {
        }

        protected virtual string ServiceName
        {
            get { return "ClientService"; }
        }

        public ClientDto Register(ClientDto clientDto)
        {
            var restResponse = JarvisRestClient.Execute<ResultDto<ClientDto>>(new RegisterClientRequest() {ClientDto = clientDto}, "POST");

            JarvisRestClient.CheckForException(restResponse.ResponseStatus);
            return restResponse.Result;
        }

        public void Logon(ClientDto clientDto)
        {
            var restResponse = JarvisRestClient.Execute<ResultDto>(new LoginClientRequest() { ClientDto = clientDto }, "PUT");
            JarvisRestClient.CheckForException(restResponse.ResponseStatus);
        }

        public void Logoff(ClientDto clientDto)
        {
            var restResponse = JarvisRestClient.Execute<ResultDto>(new LogoffClientRequest() { ClientDto = clientDto }, "PUT");
            JarvisRestClient.CheckForException(restResponse.ResponseStatus);
        }
    }
}