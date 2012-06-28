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
using System.Threading;
using jarvis.client.common.ServiceClients;
using jarvis.common.dtos;
using jarvis.common.dtos.Eventhandling;
using jarvis.common.dtos.Requests;
using jarvis.common.dtos.Workflow;
using log4net;
using log4net.Config;

namespace EventHandler
{
    internal class Program
    {
        public static DateTime lastCheck = DateTime.UtcNow;

        public static JarvisRestClient _client = new JarvisRestClient(LogManager.GetLogger("eventhandler"))
                                                     {
                                                         //BaseUrl = "http://localhost:7778/"
                                                         BaseUrl = "http://10.140.0.2:7778/"
                                                     };

        private static void Main(string[] args)
        {

            XmlConfigurator.Configure();
            Thread.Sleep(30000);
            while (true)
            {
                Thread.Sleep(10000);
                Do();
            }
        }

        private static void Do()
        {
            var eventHandlers = GetEventhandlers();
            var events = GetEvents();
            lastCheck = DateTime.UtcNow;

            HandleEvents(events, eventHandlers);
        }

        private static void HandleEvents(List<EventDto> events, List<EventHandlerDto> eventHandlers)
        {
            if (events == null)
            {
                return;
            }

            foreach (var eventDto in events)
            {
                var hittedEventHandler = from eh in eventHandlers
                                         where (eh.EventGroupType == null || eh.EventGroupType  == eventDto.EventGroupType)
                                               && (eh.EventType == null || eh.EventType == eventDto.EventType)
                                         select eh;

                foreach (var eventHandlerDto in hittedEventHandler)
                {

                    var workflowQueueDto = new WorkflowQueueDto
                                               {
                                                   EventHandlerId = eventHandlerDto.Id,
                                                   DefinedWorkflowId = eventHandlerDto.DefinedWorkflowId,
                                                   EventId = eventDto.Id
                                               };
                    _client.Execute<ResultDto>(new AddWorkflowInQueueRequest(){WorkflowQueueDto = workflowQueueDto});
                }
            }
        }

        private static List<EventDto> GetEvents()
        {

            var restResponse = _client.Execute<ResultDto<List<EventDto>>>(new GetAllEventsSinceRequest(){Ticks = lastCheck.Ticks.ToString()});
            return restResponse.Result;
        }

        private static List<EventHandlerDto> GetEventhandlers()
        {

            var result = _client.Execute<ResultDto<List<EventHandlerDto>>>(new GetEventHandlerRequest());

            return result.Result;
        }
    }
}