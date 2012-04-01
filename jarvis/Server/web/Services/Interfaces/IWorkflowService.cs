using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using jarvis.common.dtos.Workflow;

namespace jarvis.server.web.services
{
    [ServiceContract]
    public interface IWorkflowService
    {
        [OperationContract]
        [WebGet(UriTemplate = "workflow", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        RunnedWorkflowDto GetWorkflowToExecute();
    }
}
