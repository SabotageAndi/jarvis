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
using System.ServiceModel;
using jarvis.common.domain;
using jarvis.common.dtos;
using jarvis.server.common.Database;
using jarvis.server.entities.Management;

namespace jarvis.server.services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, AddressFilterMode = AddressFilterMode.Any)]
    public class ServerStatusService : IServerStatusService
    {
        private readonly ServerStatus _serverStatus;
        private readonly ITransactionProvider _transactionProvider;

        public ServerStatusService(ServerStatus serverStatus, ITransactionProvider transactionProvider)
        {
            _serverStatus = serverStatus;
            _transactionProvider = transactionProvider;
        }

        public ResultDto<Boolean> IsOnline()
        {
            using (_transactionProvider.StartReadTransaction())
            {
                return new ResultDto<bool>(_serverStatus.State == State.Running); 
            }
        }
    }
}