using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;
using jarvis.server.entities.Helper;

namespace jarvis.server.entities.Workflow
{
    public class Parameter : Entity
    {
        public virtual RunnedWorkflow RunnedWorkflow { get; set; }
        public virtual String Category { get; set; }
        public virtual String Name { get; set; }
        public virtual String Value { get; set; }
    }

    public class ParameterMap : ClassMap<Parameter>
    {
        public ParameterMap()
        {
            MappingHelper.MapId(this);

            References(x => x.RunnedWorkflow);
            Map(x => x.Category);
            Map(x => x.Name);
            Map(x => x.Value);
        }
    }
}
