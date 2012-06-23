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

using System.ServiceModel;
using jarvis.common.dtos.Management;
using jarvis.server.common.Database;
using jarvis.server.model;

namespace jarvis.server.services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, AddressFilterMode = AddressFilterMode.Any)]
    public class ClientService : IClientService
    {
        private readonly IClientLogic _clientLogic;
        private readonly ITransactionProvider _transactionProvider;

        public ClientService(IClientLogic clientLogic, ITransactionProvider transactionProvider)
        {
            _clientLogic = clientLogic;
            _transactionProvider = transactionProvider;
        }

        public ClientDto RegisterClient(ClientDto clientDto)
        {
            using(_transactionProvider.StartReadWriteTransaction())
            {
                var registerClient = _clientLogic.RegisterClient(clientDto);

                _transactionProvider.CurrentScope.Commit();
                
                return registerClient;
            }
        }

        public void LogonClient(ClientDto clientDto)
        {
            using (_transactionProvider.StartReadWriteTransaction())
            {
                _clientLogic.Logon(clientDto); 
                _transactionProvider.CurrentScope.Commit();
            }
        }

        public void LogoffClient(ClientDto clientDto)
        {
            using (_transactionProvider.StartReadWriteTransaction())
            {
                _clientLogic.Logoff(clientDto); 
                _transactionProvider.CurrentScope.Commit();
            }
        }
    }
}