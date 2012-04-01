using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using jarvis.server.entities.Helper;

namespace jarvis.server.entities.Workflow
{
    public class RunnedTask : Entity
    {
        public virtual DefinedTask DefinedTask { get; set; }
        public virtual String RunCode { get; set; }

        public virtual DateTime? Starttime { get; set; }
        public virtual DateTime? Endtime { get; set; }
    }

    public class RunnedTaskMap : ClassMap<RunnedTask>
    {
        public RunnedTaskMap()
        {
            MappingHelper.MapId(this);
            References(x => x.DefinedTask);

            Map(x => x.RunCode);
            Map(x => x.Starttime);
            Map(x => x.Endtime);
        }
    }
}
