﻿// J.A.R.V.I.S. - Just a really versatile intelligent system
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
using jarvis.server.entities;
using jarvis.server.entities.Eventhandling;
using jarvis.server.repositories;

namespace jarvis.server.model
{
    public interface IEventLogic
    {
        void eventRaised(EventDto eventDto);
        List<Event> GetEvents(EventFilterCriteria eventFilterCriteria);
        List<Event> GetLastEvents();
        List<EventDto> GetAllEventsSince(EventFilterCriteria eventFilterCriteria);
    }

    public class EventLogic : IEventLogic
    {
        private readonly IEventRepository _eventRepository;

        public EventLogic(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
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

            _eventRepository.saveTrigger(raisedEvent);
        }

        public List<Event> GetEvents(EventFilterCriteria eventFilterCriteria)
        {
            return _eventRepository.GetEvents(eventFilterCriteria).ToList();
        }

        public List<Event> GetLastEvents()
        {
            return _eventRepository.GetEvents(new EventFilterCriteria()).OrderByDescending(e => e.TriggeredDate).ToList();
        }

        public List<EventDto> GetAllEventsSince(EventFilterCriteria eventFilterCriteria)
        {
            return
                _eventRepository.GetEvents(new EventFilterCriteria() { MinTriggeredDate = eventFilterCriteria.MinTriggeredDate }).OrderBy(e => e.TriggeredDate).Select(
                    e => new EventDto()
                    {
                        TriggeredDate = e.TriggeredDate,
                        EventGroupTypes = e.EventGroupType,
                        EventType = e.EventType,
                        Data = e.Data
                    }).ToList();

        }
    }
}