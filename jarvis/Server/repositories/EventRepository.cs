// J.A.R.V.I.S. - Just a really versatile intelligent system
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

using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using jarvis.common.dtos;
using jarvis.server.common.Database;
using jarvis.server.entities;
using jarvis.server.entities.Eventhandling;

namespace jarvis.server.repositories
{
    public interface IEventRepository
    {
        void saveTrigger(Event raisedEvent);
        IQueryable<Event> GetEvents(EventFilterCriteria eventFilterCriteria);
    }

    public class EventRepository : RepositoryBase, IEventRepository
    {
        public EventRepository(ITransactionProvider transactionProvider) : base(transactionProvider)
        {
        }

        public void saveTrigger(Event raisedEvent)
        {
            CurrentSession.SaveOrUpdate(raisedEvent);
        }

        public IQueryable<Event> GetEvents(EventFilterCriteria eventFilterCriteria)
        {
            var events = CurrentSession.Query<Event>();

            if (eventFilterCriteria.MaxTriggeredDate.HasValue)
            {
                events = events.Where(e => e.TriggeredDate <= eventFilterCriteria.MaxTriggeredDate);
            }

            if (eventFilterCriteria.MinTriggeredDate.HasValue)
            {
                events = events.Where(e => e.TriggeredDate >= eventFilterCriteria.MinTriggeredDate);
            }

            return events;
        }
    }
}