using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jarvis.client.common.ServiceClients;
using jarvis.common.domain;
using jarvis.common.dtos.Management;
using log4net;

namespace jarvis.client.common
{
    public class Client
    {
        private ILog _log = LogManager.GetLogger("client");

        private readonly IClientService _clientService;

        private ClientDto _clientDto;

        public Client(IClientService clientService)
        {
            _clientService = clientService;
        }

        public void Init()
        {
            if (!isAlreadyRegistered())
            {
                _clientDto = _clientService.Register(GetClientDto());
                _log.InfoFormat("Client {0} ({1}) with Id {2} registered", _clientDto.Name, _clientDto.Hostname, _clientDto.Id);
            }

            Logon();
        }

        private void Logon()
        {
            _clientService.Logon(GetClientDto());
            _log.Info("Client logged in");
        }

        public void Run()
        {
            
        }

        private bool isAlreadyRegistered()
        {
            return false;
        }

        private ClientDto GetClientDto()
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
    }
}
