using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using jarvis.common.dtos.Workflow;
using jarvis.server.model;

namespace jarvis.server.web.services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IWorkflowLogic _workflowLogic;

        public WorkflowService(IWorkflowLogic workflowLogic)
        {
            _workflowLogic = workflowLogic;
        }

        public RunnedWorkflowDto GetWorkflowToExecute()
        {
            return _workflowLogic.GetWorkflowToExecute();
        }
    }
}
