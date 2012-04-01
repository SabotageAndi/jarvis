using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using jarvis.server.entities.Helper;

namespace jarvis.server.entities.Workflow
{
    public class RunnedNextWorkflowStep : Entity
    {
        public virtual DefinedNextWorkflowStep DefinedNextWorkflowStep { get; set; }
        public virtual RunnedWorkflow RunnedWorkflow { get; set; }
        public virtual RunnedWorkflowStep NextStep { get; set; }
        public virtual RunnedWorkflowStep PreviousStep { get; set; }
    }

    public class RunnedNextWorkflowStepMap : ClassMap<RunnedNextWorkflowStep>
    {
        public RunnedNextWorkflowStepMap()
        {
            MappingHelper.MapId(this);

            References(x => x.DefinedNextWorkflowStep);
            References(x => x.RunnedWorkflow);
            References(x => x.NextStep);
            References(x => x.PreviousStep);
        }
    }
}
