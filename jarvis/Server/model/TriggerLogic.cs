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
using jarvis.common.dtos;
using jarvis.server.entities;
using jarvis.server.repositories;

namespace jarvis.server.model
{
    public interface ITriggerLogic
    {
        void eventRaised(EventDto eventDto);
        List<Event> GetEvents(EventFilterCriteria eventFilterCriteria);
        List<Event> GetLastEvents();
    }

    public class TriggerLogic : ITriggerLogic
    {
        private readonly ITriggerRepository _triggerRepository;

        public TriggerLogic(ITriggerRepository triggerRepository)
        {
            _triggerRepository = triggerRepository;
        }

        public void eventRaised(EventDto eventDto)
        {
            var raisedEvent = new Event()
                                   {
                                       EventGroupType = eventDto.EventGroupTypes,
                                       EventType = eventDto.EventType,
                                       TriggeredDate = eventDto.TriggeredDate,
                                       Data = eventDto.Data
                                   };

            _triggerRepository.saveTrigger(raisedEvent);
        }

        public List<Event> GetEvents(EventFilterCriteria eventFilterCriteria)
        {
            return _triggerRepository.GetEvents(eventFilterCriteria).ToList();
        }

        public List<Event> GetLastEvents()
        {
            return _triggerRepository.GetEvents(new EventFilterCriteria()).OrderByDescending(e => e.TriggeredDate).ToList();
        }
    }
}