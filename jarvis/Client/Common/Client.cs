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

using jarvis.client.common.ServiceClients;
using jarvis.common.domain;
using jarvis.common.dtos.Management;
using log4net;

namespace jarvis.client.common
{
    public class Client
    {
        private readonly IClientService _clientService;
        private readonly IConfiguration _configuration;
        private readonly ILog _log = LogManager.GetLogger("client");

        private ClientDto _clientDto;

        public Client(IClientService clientService, IConfiguration configuration)
        {
            _clientService = clientService;
            _configuration = configuration;
        }

        public ClientDto ClientDto
        {
            get
            {
                if (_clientDto == null)
                {
                    _clientDto = new ClientDto()
                                     {
                                         Hostname = "localhost",
                                         Type = ClientTypeEnum.Windows,
                                         Name = "dev"
                                     };
                }
                return _clientDto;
            }
            set { _clientDto = value; }
        }

        public void Init()
        {
            LoadLocalClientInformation();

            if (!isAlreadyRegistered())
            {
                ClientDto = _clientService.Register(ClientDto);
                _log.InfoFormat("Client {0} ({1}) with Id {2} registered", _clientDto.Name, _clientDto.Hostname, _clientDto.Id);
            }
            

            Logon();


            SaveSettings();
            Logoff();
        }

        private void LoadLocalClientInformation()
        {
            ClientDto.Id = _configuration.ClientId.Value;
             
        }

        private void SaveSettings()
        {
            _configuration.ClientId = ClientDto.Id;
            _configuration.Save();
        }

        private void Logoff()
        {
            _clientService.Logoff(ClientDto);
            _log.Info("Client logged off");
        }

        private void Logon()
        {
            _clientService.Logon(ClientDto);
            _log.Info("Client logged in");
        }

        public void Run()
        {
        }

        private bool isAlreadyRegistered()
        {
            return ClientDto.Id > 0;
        }
    }
}