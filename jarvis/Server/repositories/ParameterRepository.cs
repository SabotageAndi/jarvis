using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jarvis.server.common.Database;
using jarvis.server.entities.Workflow;

namespace jarvis.server.repositories
{
    public interface IParameterRepository : IRepositoryBase<Parameter>
    {
    }

    public class ParameterRepository : RepositoryBase<Parameter>, IParameterRepository
    {
        public ParameterRepository(ITransactionProvider transactionProvider) : base(transactionProvider)
        {
        }
    }
}
