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
using jarvis.common.dtos.Eventhandling;
using jarvis.common.dtos.Workflow;
using jarvis.server.repositories;

namespace jarvis.server.model
{
    public interface IEventHandlingLogic
    {
        List<EventHandlerDto> GetAllEventHandler();
        void AddEntryInWorkflowQueue(WorkflowQueueDto workflowQueueDto);
    }

    public class EventHandlingLogic : IEventHandlingLogic
    {
        private readonly IDefinedWorkflowRepository _definedWorkflowRepository;
        private readonly IEventHandlerRepository _eventHandlerRepository;
        private readonly IWorkflowQueueRepository _workflowQueueRepository;
        private readonly IEventRepository _eventRepository;

        public EventHandlingLogic(IEventHandlerRepository eventHandlerRepository, IDefinedWorkflowRepository definedWorkflowRepository,
                                  IWorkflowQueueRepository workflowQueueRepository, IEventRepository eventRepository)
        {
            _eventHandlerRepository = eventHandlerRepository;
            _definedWorkflowRepository = definedWorkflowRepository;
            _workflowQueueRepository = workflowQueueRepository;
            _eventRepository = eventRepository;
        }

        public List<EventHandlerDto> GetAllEventHandler()
        {
            return _eventHandlerRepository.GetAllEventHandler().Select(e => new EventHandlerDto()
                                                                                {
                                                                                    Id = e.Id,
                                                                                    EventGroupTypes = e.EventGroupTypes,
                                                                                    EventType = e.EventType,
                                                                                    DefinedWorkflowId =
                                                                                        e.DefinedWorkflow == null
                                                                                            ? -1
                                                                                            : e.DefinedWorkflow.Id
                                                                                }).ToList();
        }

        public void AddEntryInWorkflowQueue(WorkflowQueueDto workflowQueueDto)
        {
            var workflow =
                _definedWorkflowRepository.GetWorkflow(new DefinedWorkflowFilterCriteria()
                                                           {
                                                               Id = workflowQueueDto.DefinedWorkflowId
                                                           });


            var workflowQueue = _workflowQueueRepository.Create();
            workflowQueue.DefinedWorkflow = workflow;
            workflowQueue.QueueDate = DateTime.UtcNow;
            workflowQueue.Event = _eventRepository.GetById(workflowQueueDto.EventId);

            _workflowQueueRepository.Save(workflowQueue);
        }
    }
}