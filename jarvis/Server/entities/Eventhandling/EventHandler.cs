using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using jarvis.common.domain;
using jarvis.server.entities.Helper;
using jarvis.server.entities.Workflow;

namespace jarvis.server.entities.Eventhandling
{
    public class EventHandler : Entity
    {
        public virtual DefinedWorkflow DefinedWorkflow { get; set; }
        public virtual EventGroupTypes? EventGroupTypes { get; set; }
        public virtual EventType? EventType { get; set; }
    }

    public class EventHandlerMap : ClassMap<EventHandler>
    {
        public EventHandlerMap()
        {
            MappingHelper.MapId(this);
            Map(x => x.EventGroupTypes);
            Map(x => x.EventType);
            References(x => x.DefinedWorkflow);
        }
    }
}
