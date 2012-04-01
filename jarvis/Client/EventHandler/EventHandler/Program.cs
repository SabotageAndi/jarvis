using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using RestSharp;
using RestSharp.Contrib;
using RestSharp.Serializers;
using jarvis.client.common;
using jarvis.common.dtos;
using jarvis.common.dtos.Eventhandling;
using jarvis.common.dtos.Workflow;

namespace EventHandler
{
    class Program
    {
        public static DateTime lastCheck = DateTime.UtcNow;
        public static RestClient _client = new RestClient("http://localhost:5368/Services/EventHandlingService.svc/");

        static void Main(string[] args)
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
                return;

            foreach (var eventDto in events)
            {
                var hittedEventHandler = from eh in eventHandlers
                                         where (eh.EventGroupTypes == null || eh.EventGroupTypes == eventDto.EventGroupTypes)
                                               && (eh.EventType == null || eh.EventType == eventDto.EventType)
                                         select eh;

                foreach (var eventHandlerDto in hittedEventHandler)
                {
                    var addWorkflowQueueRequest = RestRequestFactory.Create("workflowqueue", Method.POST);

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
            var getEvents = RestRequestFactory.Create("events/{ticks}", Method.GET);

            getEvents.AddParameter("ticks", lastCheck.Ticks, ParameterType.UrlSegment);

            var restResponse = _client.Execute<List<EventDto>>(getEvents);
            return restResponse.Data;
        }

        private static List<EventHandlerDto> GetEventhandlers()
        {
            var getEventHandler = RestRequestFactory.Create("eventhandler", Method.GET);

            var result = _client.Execute<List<EventHandlerDto>>(getEventHandler);

            return result.Data;
        }
    }
}
