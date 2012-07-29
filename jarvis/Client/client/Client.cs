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
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Threading;
using Ninject;
using jarvis.client.common.ServiceClients;
using jarvis.common.domain;
using jarvis.common.dtos.Management;
using log4net;

namespace jarvis.client.common
{
    public abstract class Client
    {
        public delegate void OnShutdownDelegate();

        private readonly List<Assembly> _addins = new List<Assembly>();

        private readonly IClientService _clientService;
        private readonly IConfiguration _configuration;
        private readonly ILog _log;
        private readonly IServerStatusService _serverStatusService;

        public ILog Log
        {
            get { return _log; }
        }

        private ClientDto _clientDto;

        public Client(IClientService clientService, IConfiguration configuration, IServerStatusService serverStatusService, ILog log)
        {
            State = State.Instanciated;
            _clientService = clientService;
            _configuration = configuration;
            _serverStatusService = serverStatusService;
            _log = log;
        }

        public static IKernel Container { get; private set; }

        public State State { get; set; }

        public List<Assembly> Addins
        {
            get { return _addins; }
        }

        public static Client Current
        {
            get { return Container.Get<Client>(); }
        }

        public virtual bool EnableAddins
        {
            get { return true; }
        }

        protected abstract ClientTypeEnum Type { get; }

        private ClientDto ClientDto
        {
            get
            {
                if (_clientDto == null)
                {
                    _clientDto = new ClientDto()
                                     {
                                         Hostname = String.Format("http://{0}:{1}/", GetLocalHostname(), _configuration.LocalPort),
                                         Type = Type, 
                                         Name = _configuration.Name
                                     };
                }
                return _clientDto;
            }
            set { _clientDto = value; }
        }

        private string GetLocalHostname()
        {
            var ips = Dns.GetHostAddresses(Dns.GetHostName()).ToList();
            ips = ips.Where(ip => ip.AddressFamily != AddressFamily.InterNetworkV6).ToList();
            var firstIp = ips.Where(ip => ip.ToString() != "127.0.0.1").FirstOrDefault();

            if (firstIp == null)
                return "localhost";

            return firstIp.ToString();
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

        public virtual void Init(IKernel container)
        {
            Thread.Sleep(10000);

            if (State >= State.Initialized)
            {
                throw new Exception("Client already initialized");
            }

            Container = container;

            AddAddInConfigHandling();
            LoadLocalClientInformation();

            if (EnableAddins)
            {
                LoadAddins();
            }

            CheckIfServerIsOnlineAndWait();

            if (!isAlreadyRegistered())
            {
                ClientDto = _clientService.Register(ClientDto);
                _log.InfoFormat("Client {0} ({1}) with Id {2} registered", _clientDto.Name, _clientDto.Hostname, _clientDto.Id);
                _configuration.ClientId = ClientDto.Id;
                SaveSettings();
            }
            ClientDto.Hostname = GetLocalHostname();

            Logon();


            State = State.Initialized;
        }

        private void AddAddInConfigHandling()
        {
            AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs e)
                                                           {
                                                               var requestedName = new AssemblyName(e.Name);

                                                               var addinAssembly = Addins.Where(a => a.GetName().Name == requestedName.Name).SingleOrDefault ();
                                                               return addinAssembly;
                                                           };
        }

        private void LoadAddins()
        {
            var addinFiles = Directory.GetFiles(_configuration.AddinPath).Where(f => f.EndsWith(".dll"));

            var basePath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            foreach (var addinFile in addinFiles)
            {
                var path = Path.Combine(basePath, addinFile);
                _addins.Add(Assembly.LoadFile(path));
            }
        }

        private void CheckIfServerIsOnlineAndWait()
        {
            while (true)
            {
                if (CheckIfServerIsOnline())
                {
                    break;
                }

                Console.WriteLine("Server not online, waiting to start ...");

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

        protected static IEnumerable<Type> GetAddinTypes(Assembly addin, Type addinType)
        {
            return addin.GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(addinType));
        }
    }
}