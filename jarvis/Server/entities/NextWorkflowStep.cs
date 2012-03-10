using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace jarvis.server.entities
{
    public class NextWorkflowStep : Entity
    {
        public virtual WorkflowStep NextStep { get; set; }
        public virtual WorkflowStep PreviousStep { get; set; }
    }

    public class NextWorkflowStepMap : ClassMap<NextWorkflowStep>
    {
        public NextWorkflowStepMap()
        {
            Id(x => x.Id).CustomSqlType("Serial").GeneratedBy.Native();
            References(x => x.NextStep);
            References(x => x.PreviousStep);
        }
    }
}
