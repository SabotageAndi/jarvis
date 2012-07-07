using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jarvis.common.dtos;
using jarvis.common.dtos.Eventhandling;
using jarvis.common.dtos.Requests;
using jarvis.common.dtos.Workflow;

namespace jarvis.client.common.ServiceClients
{
    public interface IEventHandlingService
    {
        List<EventHandlerDto> GetEventhandlers();
        List<EventDto> GetEvents(DateTime lastCheck);
        ResultDto AddWorkflowToQueue(WorkflowQueueDto workflowQueueDto);
    }

    public class EventHandlingService : ServiceBase, IEventHandlingService
    {
        public EventHandlingService(IJarvisRestClient jarvisRestClient, IConfiguration configuration) : base(jarvisRestClient, configuration)
        {
        }

        public List<EventHandlerDto> GetEventhandlers()
        {
            var result = JarvisRestClient.Execute<ResultDto<List<EventHandlerDto>>>(new GetEventHandlerRequest());

            JarvisRestClient.CheckForException(result.ResponseStatus);

            return result.Result;
        }

        public List<EventDto> GetEvents(DateTime lastCheck)
        {
            var ticks = lastCheck.Ticks.ToString();

            var restResponse = JarvisRestClient.Execute<ResultDto<List<EventDto>>>(new GetAllEventsSinceRequest() { Ticks = ticks });
            JarvisRestClient.CheckForException(restResponse.ResponseStatus);

            return restResponse.Result;
        }

        public ResultDto AddWorkflowToQueue(WorkflowQueueDto workflowQueueDto)
        {
            var restResponse = JarvisRestClient.Execute<ResultDto>(new AddWorkflowInQueueRequest {WorkflowQueueDto = workflowQueueDto});
            JarvisRestClient.CheckForException(restResponse.ResponseStatus);
            return restResponse;
        }
    }
}
