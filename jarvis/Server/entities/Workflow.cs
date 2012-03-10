using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace jarvis.server.entities
{
    public class Workflow : Entity
    {
        public virtual IList<WorkflowStep> WorkflowSteps { get; set; }
    }

    public class WorkflowMap : ClassMap<Workflow>
    {
        public WorkflowMap()
        {
            Id(x => x.Id).CustomSqlType("Serial").GeneratedBy.Native();
            HasMany(x => x.WorkflowSteps);
        }
    }
}
