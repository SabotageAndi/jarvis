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
using System.ServiceModel;
using jarvis.common.dtos.Eventhandling;
using jarvis.common.dtos.Workflow;
using jarvis.server.common.Database;
using jarvis.server.model;

namespace jarvis.server.services
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, AddressFilterMode = AddressFilterMode.Any)]
    public class EventHandlingService : IEventHandlingService
    {
        private readonly IEventHandlingLogic _eventHandlingLogic;
        private readonly IEventLogic _eventLogic;
        private readonly ITransactionProvider _transactionProvider;

        public EventHandlingService(IEventHandlingLogic eventHandlingLogic, IEventLogic eventLogic, ITransactionProvider transactionProvider)
        {
            _eventHandlingLogic = eventHandlingLogic;
            _eventLogic = eventLogic;
            _transactionProvider = transactionProvider;
        }

        public List<EventHandlerDto> GetAllEventhandler()
        {
            using (_transactionProvider.StartReadTransaction())
            {
                return _eventHandlingLogic.GetAllEventHandler(); 
            }
        }

        public List<EventDto> GetAllEventsSince(String ticks)
        {
            using (_transactionProvider.StartReadWriteTransaction())
            {
                var date = new DateTime(Convert.ToInt64(ticks));

                var allEventsSince = _eventLogic.GetAllEventsSince(date);

                _transactionProvider.CurrentScope.Commit();
                return allEventsSince; 
            }
        }

        public void CreateNewItemInWorkflowQueue(WorkflowQueueDto workflowQueueDto)
        {
            using (_transactionProvider.StartReadWriteTransaction())
            {
                _eventHandlingLogic.AddEntryInWorkflowQueue(workflowQueueDto); 
                _transactionProvider.CurrentScope.Commit();
            }
        }
    }
}