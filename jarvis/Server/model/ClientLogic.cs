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
using jarvis.common.dtos.Management;
using jarvis.server.common.Database;
using jarvis.server.repositories;

namespace jarvis.server.model
{
    public interface IClientLogic
    {
        ClientDto RegisterClient(ClientDto clientDto);
        void Logon(ClientDto clientDto);
        void Logoff(ClientDto clientDto);
    }

    public class ClientLogic : IClientLogic
    {
        private readonly IClientRepository _clientRepository;
        private readonly ITransactionProvider _transactionProvider;

        public ClientLogic(IClientRepository clientRepository, ITransactionProvider transactionProvider)
        {
            _clientRepository = clientRepository;
            _transactionProvider = transactionProvider;
        }

        public ClientDto RegisterClient(ClientDto clientDto)
        {
            var client = _clientRepository.Create();
            client.Name = clientDto.Name;
            client.Hostname = clientDto.Hostname;
            client.Type = clientDto.Type;



            _clientRepository.Save(client);

            _transactionProvider.CurrentScope.Flush();

            _clientRepository.Refresh(client);

            clientDto.Id = client.Id;

            return clientDto;
        }

        public void Logon(ClientDto clientDto)
        {
            SetIsOnlineState(clientDto, true);
        }

        public void Logoff(ClientDto clientDto)
        {
            SetIsOnlineState(clientDto, false);
        }

        private void SetIsOnlineState(ClientDto clientDto, bool isOnline)
        {
            var client = _clientRepository.GetById(clientDto.Id);
            
            if (client == null)
            {
                throw new Exception("client not registered");
            }

            client.IsOnline = isOnline;
            _clientRepository.Save(client);
        }
    }
}