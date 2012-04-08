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
        Parameter Create(RunnedWorkflow runnedWorkflow, string category, string name, string value);
    }

    public class ParameterRepository : RepositoryBase<Parameter>, IParameterRepository
    {
        public ParameterRepository(ITransactionProvider transactionProvider) : base(transactionProvider)
        {
        }

        public Parameter Create(RunnedWorkflow runnedWorkflow, string category, string name, string value)
        {
            var parameter = Create();
            parameter.RunnedWorkflow = runnedWorkflow;
            parameter.Category = category;
            parameter.Name = name;
            parameter.Value = value;

            return parameter;
        }
    }
}
