using System.Runtime.Serialization;
using jarvis.common.dtos.Workflow;

namespace jarvis.common.dtos.Requests
{
    [DataContract]
    public class AddWorkflowInQueueRequest : Request
    {
        [DataMember]
        public WorkflowQueueDto WorkflowQueueDto { get; set; }
    }
}