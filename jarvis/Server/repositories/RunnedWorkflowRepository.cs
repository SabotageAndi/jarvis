using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jarvis.server.common.Database;
using jarvis.server.entities.Workflow;

namespace jarvis.server.repositories
{
    public interface IRunnedWorkflowRepository : IRepositoryBase<RunnedWorkflow>
    {
    }

    public class RunnedWorkflowRepository : RepositoryBase<RunnedWorkflow>, IRunnedWorkflowRepository
    {
        public RunnedWorkflowRepository(ITransactionProvider transactionProvider) : base(transactionProvider)
        {
        }
    }
}
