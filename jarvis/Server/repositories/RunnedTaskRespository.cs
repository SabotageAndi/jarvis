using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jarvis.server.common.Database;
using jarvis.server.entities.Workflow;

namespace jarvis.server.repositories
{
    public interface IRunnedTaskRepository : IRepositoryBase<RunnedTask>
    {
    }

    public class RunnedTaskRespository : RepositoryBase<RunnedTask>, IRunnedTaskRepository
    {
        public RunnedTaskRespository(ITransactionProvider transactionProvider) : base(transactionProvider)
        {
        }
    }
}
