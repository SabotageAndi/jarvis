using System.Linq;
using NHibernate.Linq;
using jarvis.server.common.Database;
using jarvis.server.entities.Workflow;

namespace jarvis.server.repositories
{

    public class DefinedWorkflowFilterCriteria
    {
        public int Id { get; set; }
    }

    public interface IDefinedWorkflowRepository
    {
        DefinedWorkflow GetWorkflow(DefinedWorkflowFilterCriteria definedWorkflowFilterCriteria);
    }


    public class DefinedWorkflowRepository : RepositoryBase, IDefinedWorkflowRepository
    {
        public DefinedWorkflowRepository(ITransactionProvider transactionProvider) : base(transactionProvider)
        {
        }


        public DefinedWorkflow GetWorkflow(DefinedWorkflowFilterCriteria definedWorkflowFilterCriteria)
        {
            return CurrentSession.Query<DefinedWorkflow>().Where(df => df.Id == definedWorkflowFilterCriteria.Id).SingleOrDefault();
        }
    }
}