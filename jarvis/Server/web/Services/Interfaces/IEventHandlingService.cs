using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using jarvis.common.dtos;
using jarvis.common.dtos.Eventhandling;
using jarvis.common.dtos.Workflow;
using jarvis.server.entities.Eventhandling;
using jarvis.server.repositories;

namespace jarvis.server.web.services
{
    [ServiceContract]
    
    interface IEventHandlingService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/eventhandler/", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "GET")]
        List<EventHandlerDto> GetAllEventhandler();

        [OperationContract]
        [WebInvoke(UriTemplate = "/events/{date}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "GET")]
        List<EventDto> GetAllEventsSince(DateTime date);
        
        [OperationContract]
        [WebInvoke(UriTemplate = "/workflowqueue/", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        void CreateNewItemInWorkflowQueue(WorkflowQueueDto workflowQueueDto);
    }
}
