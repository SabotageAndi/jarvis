using System;
using FluentNHibernate.Mapping;
using jarvis.server.entities.Helper;

namespace jarvis.server.entities.Workflow
{
    public class WorkflowQueue : Entity
    {
        public virtual DateTime QueueDate { get; set; }
        public virtual DateTime? StartDate { get; set; }
        public virtual DateTime? EndDate { get; set; }
        public virtual DefinedWorkflow DefinedWorkflow { get; set; }
        public virtual RunnedWorkflow RunnedWorkflow { get; set; }
    }

    public class WorkflowQuereMap : ClassMap<WorkflowQueue>
    {
        public WorkflowQuereMap()
        {
            MappingHelper.MapId(this);
            Map(x => x.QueueDate);
            Map(x => x.StartDate);
            Map(x => x.EndDate);
            References(x => x.DefinedWorkflow);
            References(x => x.RunnedWorkflow);
        }
    }
}
