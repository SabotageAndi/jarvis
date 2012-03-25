using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using jarvis.server.entities.Helper;

namespace jarvis.server.entities.Workflow
{
    public class RunnedWorkflow : Entity
    {
        public virtual DefinedWorkflow DefinedWorkflow { get; set; }
    }


    public class  RunnedWorkflowMap : ClassMap<RunnedWorkflow>
    {
        public RunnedWorkflowMap()
        {
            MappingHelper.MapId(this);
            References(x => x.DefinedWorkflow);
        }
    }
}
