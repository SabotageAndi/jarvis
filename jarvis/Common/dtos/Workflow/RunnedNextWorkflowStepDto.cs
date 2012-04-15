namespace jarvis.common.dtos.Workflow
{
    public class RunnedNextWorkflowStepDto
    {
        public int Id { get; set; }

        public int? NextStepId { get; set; }

        public int? PreviousStepId { get; set; }
    }
}