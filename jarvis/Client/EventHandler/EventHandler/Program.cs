using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Serializers;
using jarvis.common.dtos;
using jarvis.common.dtos.Eventhandling;
using jarvis.common.dtos.Workflow;
using JsonSerializer = RestSharp.Serializers.JsonSerializer;

namespace EventHandler
{
    class Program
    {
        public static DateTime lastCheck = DateTime.UtcNow;
        public static RestClient _client = new RestClient("http://localhost:5368/Services/EventHandlingService.svc/");

        static void Main(string[] args)
        {
            while (true)
            {
                Thread.Sleep(10000);
                Do();
                lastCheck = DateTime.UtcNow;
            }
        }

        private static void Do()
        {
            var getEventHandler = new RestRequest("eventhandler", Method.GET);
            getEventHandler.RequestFormat = DataFormat.Json;

            var result = _client.Execute<List<EventHandlerDto>>(getEventHandler);

            var eventHandlers = result.Data;

            var getEvents = new RestRequest("events", Method.POST);
            getEvents.JsonSerializer = new JsonSerializer(Newtonsoft.Json.JsonSerializer.Create(new JsonSerializerSettings()
            {
                MissingMemberHandling = MissingMemberHandling.Ignore,
                NullValueHandling = NullValueHandling.Include,
                DefaultValueHandling = DefaultValueHandling.Include,
                DateFormatHandling = DateFormatHandling.MicrosoftDateFormat
            }));
            
            getEvents.RequestFormat = DataFormat.Json;
            getEvents.AddBody(new EventFilterCriteria {MinTriggeredDate = lastCheck});

            var restResponse = _client.Execute<List<EventDto>>(getEvents);
            var events = restResponse.Data;

            if (events != null)
            {
                foreach (var eventDto in events)
                {
                    var hittedEventHandler = from eh in eventHandlers
                                             where (eh.EventGroupTypes == null || eh.EventGroupTypes == eventDto.EventGroupTypes)
                                                   && (eh.EventType == null || eh.EventType == eventDto.EventType)
                                             select eh;

                    foreach (var eventHandlerDto in hittedEventHandler)
                    {
                        var addWorkflowQueueRequest = new RestRequest("workflowqueue", Method.POST);
                        addWorkflowQueueRequest.RequestFormat = DataFormat.Json;

                        addWorkflowQueueRequest.AddBody(new WorkflowQueueDto
                                                            {
                                                                EventHandlerId = eventHandlerDto.Id,
                                                                DefinedWorkflowId = eventHandlerDto.DefinedWorkflowId
                                                            });

                        _client.Execute(addWorkflowQueueRequest);
                    }
                }
            }
        }
    }
}
