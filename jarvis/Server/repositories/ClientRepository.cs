﻿// J.A.R.V.I.S. - Just A Rather Very Intelligent System
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
using System.Linq;
using NHibernate.Linq;
using jarvis.common.domain;
using jarvis.server.common.Database;
using jarvis.server.entities.Management;

namespace jarvis.server.repositories
{

    public class ClientFilterCriteria
    {
        public string Name { get; set; }
        public ClientTypeEnum? Type { get; set; }

        public bool? IsOnline { get; set; }
    }

    public interface IClientRepository : IRepositoryBase<Client>
    {
        IEnumerable<Client> GetClientsByFilterCriteria(ITransactionScope transactionScope, ClientFilterCriteria clientFilterCriteria);
    }

    public class ClientRepository : RepositoryBase<Client>, IClientRepository
    {
        public IEnumerable<Client> GetClientsByFilterCriteria(ITransactionScope transactionScope, ClientFilterCriteria clientFilterCriteria)
        {
            var clients = transactionScope.CurrentSession.Query<Client>();

            if (!String.IsNullOrEmpty(clientFilterCriteria.Name))
                clients = clients.Where(c => c.Name == clientFilterCriteria.Name);

            if (clientFilterCriteria.Type.HasValue)
                clients = clients.Where(c => c.Type == clientFilterCriteria.Type);

            if (clientFilterCriteria.IsOnline.HasValue)
                clients = clients.Where(c => c.IsOnline == clientFilterCriteria.IsOnline);

            return clients;
        }
    }
}