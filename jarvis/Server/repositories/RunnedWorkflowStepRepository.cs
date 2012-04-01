using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jarvis.server.common.Database;
using jarvis.server.entities.Workflow;

namespace jarvis.server.repositories
{
    public interface IRunnedWorkflowStepRepository : IRepositoryBase<RunnedWorkflowStep>
    {
    }

    public class RunnedWorkflowStepRepository : RepositoryBase<RunnedWorkflowStep>, IRunnedWorkflowStepRepository 
    {
        public RunnedWorkflowStepRepository(ITransactionProvider transactionProvider) : base(transactionProvider)
        {
        }
    }
}
