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
using RestSharp;
using jarvis.client.common.ServiceClients;
using jarvis.common.dtos.Eventhandling;
using jarvis.common.dtos.Workflow;

namespace EventHandler
{
    internal class Program
    {
        public static DateTime lastCheck = DateTime.UtcNow;

        public static JarvisRestClient _client = new JarvisRestClient()
                                                     {
                                                         BaseUrl = "http://localhost:5368/Services/EventHandlingService.svc/"
                                                     };

        private static void Main(string[] args)
        {
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
                                         where (eh.EventGroupTypes == null || eh.EventGroupTypes == eventDto.EventGroupTypes)
                                               && (eh.EventType == null || eh.EventType == eventDto.EventType)
                                         select eh;

                foreach (var eventHandlerDto in hittedEventHandler)
                {
                    var addWorkflowQueueRequest = _client.CreateRequest("workflowqueue", Method.POST);

                    addWorkflowQueueRequest.AddBody(new WorkflowQueueDto
                                                        {
                                                            EventHandlerId = eventHandlerDto.Id,
                                                            DefinedWorkflowId = eventHandlerDto.DefinedWorkflowId
                                                        });

                    _client.Execute(addWorkflowQueueRequest);
                }
            }
        }

        private static List<EventDto> GetEvents()
        {
            var getEvents = _client.CreateRequest("events/{ticks}", Method.GET);

            getEvents.AddParameter("ticks", lastCheck.Ticks, ParameterType.UrlSegment);

            var restResponse = _client.Execute<List<EventDto>>(getEvents);
            return restResponse;
        }

        private static List<EventHandlerDto> GetEventhandlers()
        {
            var getEventHandler = _client.CreateRequest("eventhandler", Method.GET);

            var result = _client.Execute<List<EventHandlerDto>>(getEventHandler);

            return result;
        }
    }
}