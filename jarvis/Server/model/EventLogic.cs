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
using System.Linq;
using jarvis.common.dtos;
using jarvis.common.dtos.Eventhandling;
using jarvis.server.entities.Eventhandling;
using jarvis.server.repositories;

namespace jarvis.server.model
{
    public interface IEventLogic
    {
        void eventRaised(EventDto eventDto);
        List<Event> GetEvents(EventFilterCriteria eventFilterCriteria);
        List<Event> GetLastEvents();
        List<EventDto> GetAllEventsSince(DateTime date);
    }

    public class EventLogic : IEventLogic
    {
        private readonly IClientRepository _clientRepository;
        private readonly IEventRepository _eventRepository;

        public EventLogic(IEventRepository eventRepository, IClientRepository clientRepository)
        {
            _eventRepository = eventRepository;
            _clientRepository = clientRepository;
        }

        public void eventRaised(EventDto eventDto)
        {
            var client = _clientRepository.GetById(eventDto.ClientId);

            var raisedEvent = new Event()
                                  {
                                      EventGroupType = eventDto.EventGroupTypes,
                                      EventType = eventDto.EventType,
                                      TriggeredDate = eventDto.TriggeredDate,
                                      Data = eventDto.Data,
                                      Client = client
                                  };

            _eventRepository.Save(raisedEvent);
        }

        public List<Event> GetEvents(EventFilterCriteria eventFilterCriteria)
        {
            return _eventRepository.GetEvents(eventFilterCriteria).ToList();
        }

        public List<Event> GetLastEvents()
        {
            return _eventRepository.GetEvents(new EventFilterCriteria()).OrderByDescending(e => e.TriggeredDate).ToList();
        }

        public List<EventDto> GetAllEventsSince(DateTime date)
        {
            return
                _eventRepository.GetEvents(new EventFilterCriteria()
                                               {
                                                   MinTriggeredDate = date,
                                                   MaxTriggeredDate = DateTime.UtcNow
                                               }).OrderBy(
                                                   e => e.TriggeredDate).Select(
                                                       e => new EventDto()
                                                                {
                                                                    Id = e.Id,
                                                                    TriggeredDate = e.TriggeredDate,
                                                                    EventGroupTypes = e.EventGroupType,
                                                                    EventType = e.EventType,
                                                                    Data = e.Data
                                                                }).ToList();
        }
    }
}