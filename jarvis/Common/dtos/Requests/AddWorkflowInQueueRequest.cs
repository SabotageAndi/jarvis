using jarvis.common.dtos.Workflow;

namespace jarvis.common.dtos.Requests
{
    public class AddWorkflowInQueueRequest : Request
    {
        public WorkflowQueueDto WorkflowQueueDto { get; set; }
    }
}