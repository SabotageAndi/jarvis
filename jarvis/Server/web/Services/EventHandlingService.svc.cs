// J.A.R.V.I.S. - Just A Really Versatile Intelligent System
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
using System.ServiceModel;
using jarvis.common.dtos;
using jarvis.common.dtos.Eventhandling;
using jarvis.common.dtos.Workflow;
using jarvis.server.model;
using jarvis.server.repositories;

namespace jarvis.server.web.services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public class EventHandlingService : IEventHandlingService
    {
        private readonly IEventHandlingLogic _eventHandlingLogic;
        private readonly IEventLogic _eventLogic;

        public EventHandlingService(IEventHandlingLogic eventHandlingLogic, IEventLogic eventLogic)
        {
            _eventHandlingLogic = eventHandlingLogic;
            _eventLogic = eventLogic;
        }

        public List<EventHandlerDto> GetAllEventhandler()
        {
            return _eventHandlingLogic.GetAllEventHandler();
        }

        public List<EventDto> GetAllEventsSince(String ticks)
        {
            var date = new DateTime(Convert.ToInt64(ticks));

            return _eventLogic.GetAllEventsSince(date);
        }

        public void CreateNewItemInWorkflowQueue(WorkflowQueueDto workflowQueueDto)
        {
            _eventHandlingLogic.AddEntryInWorkflowQueue(workflowQueueDto);
        }
    }
}
