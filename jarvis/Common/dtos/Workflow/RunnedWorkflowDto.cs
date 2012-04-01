using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jarvis.common.dtos.Workflow
{
    public class RunnedWorkflowDto
    {
        public int Id { get; set; }

        public List<RunnedWorkflowStepDto> WorkflowSteps { get; set; }

        public List<RunnedNextWorkflowStepDto> NextWorkflowSteps { get; set; }
    }

    public class RunnedNextWorkflowStepDto
    {
        public int Id { get; set; }

        public int? NextStepId { get; set; }

        public int? PreviousStepId { get; set; }
    }

    public class RunnedWorkflowStepDto
    {
        public int Id { get; set; }

        public RunnedTaskDto RunnedTask { get; set; }
    }

    public class RunnedTaskDto
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public string RunCode { get; set; }
    }
}
