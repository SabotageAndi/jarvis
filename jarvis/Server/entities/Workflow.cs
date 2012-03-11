using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace jarvis.server.entities
{
    public class Workflow : Entity
    {
        public virtual IList<WorkflowStep> WorkflowSteps { get; set; }
        public virtual Folder ParentFolder { get; set; }
        public virtual String Name { get; set; }
    }

    public class WorkflowMap : ClassMap<Workflow>
    {
        public WorkflowMap()
        {
            Id(x => x.Id).CustomSqlType("Serial").GeneratedBy.Native();
            Map(x => x.Name);
            References(x => x.ParentFolder);

            HasMany(x => x.WorkflowSteps);
        }
    }
}
