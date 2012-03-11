﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Mapping;

namespace jarvis.server.entities
{
    public class Task : Entity
    {
        public virtual String Name { get; set; }
        public virtual Folder ParentFolder { get; set; }
    }

    public class TaskMap : ClassMap<Task>
    {
        public TaskMap()
        {
            Id(x => x.Id).CustomSqlType("Serial").GeneratedBy.Native();
            Map(x => x.Name);
            References(x => x.ParentFolder);
        }
    }
}