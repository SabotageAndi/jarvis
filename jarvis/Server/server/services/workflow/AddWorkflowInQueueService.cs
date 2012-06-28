using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceInterface;
using jarvis.common.dtos;
using jarvis.common.dtos.Requests;
using jarvis.server.common.Database;
using jarvis.server.model;

namespace jarvis.server.services.workflow
{
    class AddWorkflowInQueueService : ServiceBase<AddWorkflowInQueueRequest>
    {
        private readonly IEventHandlingLogic _eventHandlingLogic;
        private readonly ITransactionProvider _transactionProvider;

        public AddWorkflowInQueueService(ITransactionProvider transactionProvider, IEventHandlingLogic eventHandlingLogic)
        {
            _transactionProvider = transactionProvider;
            _eventHandlingLogic = eventHandlingLogic;
        }

        protected override object Run(AddWorkflowInQueueRequest request)
        {
            using (var transactionScope = _transactionProvider.StartReadWriteTransaction())
            {
                _eventHandlingLogic.AddEntryInWorkflowQueue(transactionScope, request.WorkflowQueueDto);
                transactionScope.Commit();

                return new ResultDto();
            }
        }
    }
}
