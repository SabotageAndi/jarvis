using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceInterface;
using jarvis.common.dtos;
using jarvis.common.dtos.Requests;
using jarvis.common.dtos.Workflow;
using jarvis.server.common.Database;
using jarvis.server.model;

namespace jarvis.server.services.workflow
{
    class GetWorkflowToExecuteService : ServiceBase<GetWorkflowToExecuteRequest>
    {
        private readonly IWorkflowLogic _workflowLogic;
        private readonly ITransactionProvider _transactionProvider;

        public GetWorkflowToExecuteService(IWorkflowLogic workflowLogic, ITransactionProvider transactionProvider)
        {
            _workflowLogic = workflowLogic;
            _transactionProvider = transactionProvider;
        }

        protected override object Run(GetWorkflowToExecuteRequest request)
        {
            using (var transactionScope = _transactionProvider.StartReadWriteTransaction())
            {
                var workflowToExecute = _workflowLogic.GetWorkflowToExecute(transactionScope);

                transactionScope.Commit();
                return new ResultDto<RunnedWorkflowDto>(workflowToExecute);
            }
        }
    }
}
