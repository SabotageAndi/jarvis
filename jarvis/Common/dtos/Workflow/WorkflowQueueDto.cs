using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jarvis.common.dtos.Workflow
{
    public class WorkflowQueueDto
    {
        public int EventHandlerId { get; set; }
        public int DefinedWorkflowId { get; set; }
    }
}
