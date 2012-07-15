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
using jarvis.client.common.ServiceClients;
using jarvis.common.domain;
using jarvis.common.dtos;
using jarvis.common.dtos.Eventhandling;
using jarvis.common.dtos.Requests;
using jarvis.common.dtos.Workflow;
using jarvis.common.logic;
using jarvis.server.common.Database;
using jarvis.server.entities.Management;
using jarvis.server.repositories;
using log4net;

namespace jarvis.server.model
{
    public interface IEventHandlingLogic
    {
        List<EventHandlerDto> GetAllEventHandler(ITransactionScope transactionScope);
        void AddEntryInWorkflowQueue(ITransactionScope transactionScope, WorkflowQueueDto workflowQueueDto);
    }

    public class EventHandlingLogic : IEventHandlingLogic
    {
        private readonly IDefinedWorkflowRepository _definedWorkflowRepository;
        private readonly IEventHandlerRepository _eventHandlerRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IClientLogic _clientLogic;
        private readonly ILog _log;
        private readonly IWorkflowQueueRepository _workflowQueueRepository;

        public EventHandlingLogic(IEventHandlerRepository eventHandlerRepository, IDefinedWorkflowRepository definedWorkflowRepository,
                                  IWorkflowQueueRepository workflowQueueRepository, IEventRepository eventRepository, IClientLogic clientLogic, ILog log)
        {
            _eventHandlerRepository = eventHandlerRepository;
            _definedWorkflowRepository = definedWorkflowRepository;
            _workflowQueueRepository = workflowQueueRepository;
            _eventRepository = eventRepository;
            _clientLogic = clientLogic;
            _log = log;
        }

        public List<EventHandlerDto> GetAllEventHandler(ITransactionScope transactionScope)
        {
            return _eventHandlerRepository.GetAllEventHandler(transactionScope).Select(e => new EventHandlerDto()
                                                                                {
                                                                                    Id = e.Id,
                                                                                    EventGroupType = e.EventGroupTypes,
                                                                                    EventType = e.EventType,
                                                                                    DefinedWorkflowId =
                                                                                        e.DefinedWorkflow == null
                                                                                            ? -1
                                                                                            : e.DefinedWorkflow.Id
                                                                                }).ToList();
        }

        public void AddEntryInWorkflowQueue(ITransactionScope transactionScope, WorkflowQueueDto workflowQueueDto)
        {
            var workflow =
                _definedWorkflowRepository.GetWorkflow(transactionScope, new DefinedWorkflowFilterCriteria()
                    {
                        Id = workflowQueueDto.DefinedWorkflowId
                    });


            var workflowQueue = _workflowQueueRepository.Create();
            workflowQueue.DefinedWorkflow = workflow;
            workflowQueue.QueueDate = DateTime.UtcNow;
            workflowQueue.Event = _eventRepository.GetById(transactionScope, workflowQueueDto.EventId);

            _workflowQueueRepository.Save(transactionScope, workflowQueue);

            var workerClients = _clientLogic.GetClientByFilterCriteria(transactionScope,
                                                                             new ClientFilterCriteria()
                                                                                 {Type = ClientTypeEnum.Worker, IsOnline = true});

            var eventhandlerClient = workerClients.FirstOrDefault();

            if (eventhandlerClient != null)
            {
                TriggerWorkerClient(eventhandlerClient);
            }

        }
        public void TriggerWorkerClient(Client client)
        {
            var restclient = new JarvisRestClient(_log);
            restclient.BaseUrl = client.Hostname;

            try
            {
                var result = restclient.Execute<ResultDto>(new WorkerTriggerRequest(), "POST");
                restclient.CheckForException(result.ResponseStatus);
            }
            catch (Exception exception)
            {
                ExceptionDumper.Write(exception);
            }
            //restclient.ExecuteAsync(new WorkerTriggerRequest(), o => { }, (o, exception) => _log.ErrorFormat("Error on triggering worker client: {0}", ExceptionDumper.Write(exception)), "POST");
        }
    }
}