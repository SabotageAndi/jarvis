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
using System.Threading;
using Autofac;
using jarvis.client.common.Actions;
using jarvis.client.common.ServiceClients;
using jarvis.client.common.Triggers;
using jarvis.client.common.Triggers.FileSystemTrigger;
using jarvis.common.domain;
using jarvis.common.dtos.Management;
using log4net;


namespace jarvis.client.common
{
    public class Client
    {
        public delegate void OnShutdownDelegate();

        private static IContainer _container;

        private readonly IClientService _clientService;
        private readonly IConfiguration _configuration;
        private readonly IServerStatusService _serverStatusService;
        private readonly ILog _log = LogManager.GetLogger("client");

        private ClientDto _clientDto;

        public Client(IClientService clientService, IConfiguration configuration, IServerStatusService serverStatusService)
        {
            State = State.Instanciated;
            _clientService = clientService;
            _configuration = configuration;
            _serverStatusService = serverStatusService;

        }

        public State State { get; set; }

        public static Client Current
        {
            get { return _container.Resolve<Client>(); }
        }

        private ClientDto ClientDto
        {
            get
            {
                if (_clientDto == null)
                {
                    _clientDto = new ClientDto()
                                     {
                                         Hostname = String.Format("http://localhost:{0}/", _configuration.LocalPort),
                                         Type = ClientTypeEnum.Windows,
                                         Name = _configuration.Name
                                     };
                }
                return _clientDto;
            }
            set { _clientDto = value; }
        }

        public event OnShutdownDelegate OnShutDown;

        private void OnOnShutDown()
        {
            OnShutdownDelegate handler = OnShutDown;
            if (handler != null)
            {
                handler();
            }
        }

        public virtual void Init(IContainer container)
        {
            if (State >= State.Initialized)
            {
                throw new Exception("Client already initialized");
            }

            _container = container;

            LoadLocalClientInformation();

            CheckIfServerIsOnlineAndWait();


            if (!isAlreadyRegistered())
            {
                ClientDto = _clientService.Register(ClientDto);
                _log.InfoFormat("Client {0} ({1}) with Id {2} registered", _clientDto.Name, _clientDto.Hostname, _clientDto.Id);
                _configuration.ClientId = ClientDto.Id;
                SaveSettings();
            }

            Logon();

           
            State = State.Initialized;
        }

        private void CheckIfServerIsOnlineAndWait()
        {
            while(true)
            {
                if (CheckIfServerIsOnline()) 
                    break;

                Thread.Sleep(10000);
            }
        }

        private bool CheckIfServerIsOnline()
        {
            return _serverStatusService.isOnline();
        }

        public virtual void Shutdown()
        {
            State = State.Shutdown;
            SaveSettings();
            Logoff();

            OnOnShutDown();
        }

        private void LoadLocalClientInformation()
        {
            if (_configuration.ClientId != null)
            {
                ClientDto.Id = _configuration.ClientId.Value;
            }
        }

        private void SaveSettings()
        {
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

        public virtual void Run()
        {
            State = State.Running;
        }

        private bool isAlreadyRegistered()
        {
            return ClientDto.Id > 0;
        }
    }
}