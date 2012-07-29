using System.Collections.Generic;
using System.Linq;
using System.Text;
using jarvis.common.dtos;
using jarvis.common.dtos.Eventhandling;
using jarvis.common.dtos.Requests;
using jarvis.common.dtos.Workflow;
using jarvis.common.logic;

namespace jarvis.client.common.ServiceClients
{
    public interface IEventHandlingService
    {
        List<EventHandlerDto> GetEventhandlers();
        EventDto GetEventToProcess();
        ResultDto AddWorkflowToQueue(WorkflowQueueDto workflowQueueDto);
    }

    public class EventHandlingService : ServiceBase, IEventHandlingService
    {
        public EventHandlingService(IJarvisRestClient jarvisRestClient, IConfiguration configuration)
            : base(jarvisRestClient, configuration)
        {
        }

        public List<EventHandlerDto> GetEventhandlers()
        {
            var result = JarvisRestClient.Execute<ResultDto<List<EventHandlerDto>>>(new GetEventHandlerRequest(), "GET");

            JarvisRestClient.CheckForException(result.ResponseStatus);

            return result.Result;
        }

        public EventDto GetEventToProcess()
        {
            var restResponse = JarvisRestClient.Execute<ResultDto<EventDto>>(new GetEventsToProcess(), "POST");
            JarvisRestClient.CheckForException(restResponse.ResponseStatus);

            return restResponse.Result;
        }

        public ResultDto AddWorkflowToQueue(WorkflowQueueDto workflowQueueDto)
        {
            var restResponse = JarvisRestClient.Execute<ResultDto>(new AddWorkflowInQueueRequest { WorkflowQueueDto = workflowQueueDto }, "POST");
            JarvisRestClient.CheckForException(restResponse.ResponseStatus);
            return restResponse;
        }
    }
}
