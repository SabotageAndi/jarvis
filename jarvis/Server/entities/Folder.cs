using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace jarvis.server.entities
{
    public class Folder : Entity
    {
        public virtual string Name { get; set; }
        public virtual Folder Parent { get; set; }
    }

    public class FolderMap : ClassMap<Folder>
    {
        public FolderMap()
        {
            Id(x => x.Id).CustomSqlType("Serial").GeneratedBy.Native();
            Map(x => x.Name);
            References(x => x.Parent);
        }
    }
}
