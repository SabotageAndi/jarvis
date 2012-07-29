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
using jarvis.common.domain;
using jarvis.common.dtos.Management;
using jarvis.server.common.Database;
using jarvis.server.entities.Management;
using jarvis.server.repositories;
using System.Linq;

namespace jarvis.server.model
{
    public interface IClientLogic
    {
        ClientDto RegisterClient(ITransactionScope transactionScope, ClientDto clientDto);
        void Logon(ITransactionScope transactionScope, ClientDto clientDto);
        void Logoff(ITransactionScope transactionScope, ClientDto clientDto);
        List<Client> GetClientByFilterCriteria(ITransactionScope transactionScope, ClientFilterCriteria clientFilterCriteria);
    }

    public class ClientLogic : IClientLogic
    {
        private readonly IClientRepository _clientRepository;

        public ClientLogic(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public ClientDto RegisterClient(ITransactionScope transactionScope, ClientDto clientDto)
        {
            Client client = _clientRepository.GetClientsByFilterCriteria(transactionScope, new ClientFilterCriteria() {Name = clientDto.Name}).SingleOrDefault();
            ;
            if (client == null)
            {
                client = _clientRepository.Create();
            }

            client.Name = clientDto.Name;
            client.Hostname = clientDto.Hostname;
            client.Type = clientDto.Type;


            _clientRepository.Save(transactionScope, client);

            transactionScope.Flush();

            _clientRepository.Refresh(transactionScope, client);

            clientDto.Id = client.Id;

            return clientDto;
        }

        public void Logon(ITransactionScope transactionScope, ClientDto clientDto)
        {
            SetIsOnlineState(transactionScope, clientDto, true);
        }

        public void Logoff(ITransactionScope transactionScope, ClientDto clientDto)
        {
            SetIsOnlineState(transactionScope, clientDto, false);
        }

        public List<Client> GetClientByFilterCriteria(ITransactionScope transactionScope, ClientFilterCriteria clientFilterCriteria)
        {
            return _clientRepository.GetClientsByFilterCriteria(transactionScope, clientFilterCriteria).ToList();
        }


        private void SetIsOnlineState(ITransactionScope transactionScope, ClientDto clientDto, bool isOnline)
        {
            var client = _clientRepository.GetById(transactionScope, clientDto.Id);

            if (client == null)
            {
                throw new Exception("client not registered");
            }


            client.IsOnline = isOnline;
            clientDto.Hostname = clientDto.Hostname;
            _clientRepository.Save(transactionScope, client);
        }
    }
}