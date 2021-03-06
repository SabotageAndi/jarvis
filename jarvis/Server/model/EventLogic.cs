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
using jarvis.common.domain;
using jarvis.common.dtos;
using jarvis.common.dtos.Eventhandling;
using jarvis.common.dtos.Requests;
using jarvis.common.logic;
using jarvis.server.common.Database;
using jarvis.server.entities.Eventhandling;
using jarvis.server.entities.Management;
using jarvis.server.repositories;
using log4net;

namespace jarvis.server.model
{
    public interface IEventLogic
    {
        void eventRaised(ITransactionScope transactionScope, EventDto eventDto);
        List<Event> GetEvents(ITransactionScope transactionScope, EventFilterCriteria eventFilterCriteria);
        List<Event> GetLastEvents(ITransactionScope transactionScope);
        EventDto GetEventsToProcess(ITransactionScope transactionScope);
    }

    public class EventLogic : IEventLogic
    {
        private readonly IClientRepository _clientRepository;
        private readonly IClientLogic _clientLogic;
        private readonly ILog _log;
        private readonly IEventRepository _eventRepository;

        public EventLogic(IEventRepository eventRepository, IClientRepository clientRepository, IClientLogic clientLogic, ILog log)
        {
            _eventRepository = eventRepository;
            _clientRepository = clientRepository;
            _clientLogic = clientLogic;
            _log = log;
        }

        public void eventRaised(ITransactionScope transactionScope, EventDto eventDto)
        {
            var client = _clientRepository.GetById(transactionScope, eventDto.ClientId);

            var raisedEvent = new Event()
                                  {
                                      EventGroupType = eventDto.EventGroupType,
                                      EventType = eventDto.EventType,
                                      TriggeredDate = eventDto.TriggeredDate,
                                      Data = eventDto.Data,
                                      Client = client
                                  };

            _eventRepository.Save(transactionScope, raisedEvent);

            var eventhandlerClients = _clientLogic.GetClientByFilterCriteria(transactionScope, new ClientFilterCriteria() { Type = ClientTypeEnum.Eventhandler, IsOnline = true });

            var eventhandlerClient = eventhandlerClients.FirstOrDefault();

            if (eventhandlerClient != null)
            {
                TriggerEventForEventhandlerClient(eventhandlerClient);
            }
        }

        public List<Event> GetEvents(ITransactionScope transactionScope, EventFilterCriteria eventFilterCriteria)
        {
            return _eventRepository.GetEvents(transactionScope, eventFilterCriteria).ToList();
        }

        public List<Event> GetLastEvents(ITransactionScope transactionScope)
        {
            return _eventRepository.GetEvents(transactionScope, new EventFilterCriteria()).OrderByDescending(e => e.TriggeredDate).ToList();
        }

        public EventDto GetEventsToProcess(ITransactionScope transactionScope)
        {
            var eventToProcess = _eventRepository.GetEvents(transactionScope, new EventFilterCriteria()
                {
                    IsProcessed = false
                }).OrderBy(e => e.TriggeredDate).FirstOrDefault();


            if (eventToProcess == null)
                return new EventDto(){Id = -1};

            eventToProcess.ProcessedDate = DateTime.UtcNow;

            _eventRepository.Save(transactionScope, eventToProcess);
            transactionScope.Flush();

            return new EventDto()
                    {
                        Id = eventToProcess.Id,
                        TriggeredDate = eventToProcess.TriggeredDate,
                        ProcessedDate = eventToProcess.ProcessedDate,
                        EventGroupType = eventToProcess.EventGroupType,
                        EventType = eventToProcess.EventType,
                        Data = eventToProcess.Data
                    };
        }

        public void TriggerEventForEventhandlerClient(Client client)
        {
            var restclient = new JarvisRestClient(_log);
            restclient.BaseUrl = client.Hostname;

            try
            {
                var result = restclient.Execute<ResultDto>(new EventhandlingTriggerRequest(), "POST");
                restclient.CheckForException(result.ResponseStatus);
            }
            catch (Exception exception)
            {
                ExceptionDumper.Write(exception);
            }

            //restclient.ExecuteAsync(new EventhandlingTriggerRequest(), o => { }, (o, exception) => { _log.ErrorFormat("Error on triggering eventhandler: {0}", ExceptionDumper.Write(exception)); }, "POST");
        }
    }
}