using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jarvis.server.common.Database;
using jarvis.server.entities.Workflow;

namespace jarvis.server.repositories
{
    public interface IWorkflowQueueRepository
    {
        WorkflowQueue Create();
        void Save(WorkflowQueue workflowQueue);
    }

    public class WorkflowQueueRepository : RepositoryBase, IWorkflowQueueRepository
    {
        public WorkflowQueueRepository(ITransactionProvider transactionProvider)
            : base(transactionProvider)
        {
        }

        public WorkflowQueue Create()
        {
            return new WorkflowQueue();
        }

        public void Save(WorkflowQueue workflowQueue)
        {
            CurrentSession.SaveOrUpdate(workflowQueue);
        }
    }
}
