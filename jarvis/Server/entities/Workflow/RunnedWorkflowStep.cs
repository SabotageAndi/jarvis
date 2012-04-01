using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using jarvis.server.entities.Helper;

namespace jarvis.server.entities.Workflow
{
    public class RunnedWorkflowStep : Entity
    {
        public virtual DefinedWorkflowStep DefinedWorkflowStep { get; set; }
        public virtual RunnedWorkflow RunnedWorkflow { get; set; }
        public virtual RunnedTask RunnedTask { get; set; }

        public virtual DateTime? Starttime { get; set; }
        public virtual DateTime? Endtime { get; set; }
    }

    public class RunnedWorkflowStepMap : ClassMap<RunnedWorkflowStep>
    {
        public RunnedWorkflowStepMap()
        {
            MappingHelper.MapId(this);
            References(x => x.DefinedWorkflowStep);
            References(x => x.RunnedWorkflow);
            References(x => x.RunnedTask);

            Map(x => x.Starttime);
            Map(x => x.Endtime);
        }
    }
}
