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

using System.Linq;
using NHibernate.Linq;
using jarvis.server.common.Database;
using jarvis.server.entities.Eventhandling;

namespace jarvis.server.repositories
{
    public interface IEventHandlerRepository : IRepositoryBase<EventHandler>
    {
        IQueryable<EventHandler> GetAllEventHandler(ITransactionScope transactionScope);
    }

    public class EventHandlerRepository : RepositoryBase<EventHandler>, IEventHandlerRepository
    {
        public IQueryable<EventHandler> GetAllEventHandler(ITransactionScope transactionScope)
        {
            return transactionScope.CurrentSession.Query<EventHandler>();
        }
    }
}