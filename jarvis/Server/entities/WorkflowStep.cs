using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace jarvis.server.entities
{
    public class WorkflowStep : Entity
    {
        public virtual Workflow Workflow { get; set; }
        public virtual Task Task { get; set; }

    }

    public class WorkflowStepMap : ClassMap<WorkflowStep>
    {
        public WorkflowStepMap()
        {
            Id(x => x.Id).CustomSqlType("Serial").GeneratedBy.Native();
            References(x => x.Task);
            References(x => x.Workflow);
        }
    }
}
