using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using jarvis.common.domain;

namespace jarvis.server.entities
{
    public class Event : Entity
    {
        public virtual EventGroupTypes EventGroupType { get; set; }
        public virtual EventType EventType { get; set; }
        public virtual DateTime TriggeredDate { get; set; }
    }

    public class EventMap : ClassMap<Event>
    {
        public EventMap()
        {
            Id(x => x.Id).CustomSqlType("Serial").GeneratedBy.Native();
            Map(x => x.EventGroupType);
            Map(x => x.EventType);
            Map(x => x.TriggeredDate);
        }
    }
}
