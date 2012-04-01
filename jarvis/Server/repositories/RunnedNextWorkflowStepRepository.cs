using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jarvis.server.common.Database;
using jarvis.server.entities.Workflow;

namespace jarvis.server.repositories
{
    public interface IRunnedNextWorkflowStepRepository : IRepositoryBase<RunnedNextWorkflowStep>
    {
    }

    public class RunnedNextWorkflowStepRepository:RepositoryBase<RunnedNextWorkflowStep>, IRunnedNextWorkflowStepRepository
    {
        public RunnedNextWorkflowStepRepository(ITransactionProvider transactionProvider) : base(transactionProvider)
        {
        }
    }
}
