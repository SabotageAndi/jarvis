// J.A.R.V.I.S. - Just A Rather Very Intelligent System
// Copyright (C) 2012 Andreas Willich <sabotageandi@gmail.com>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
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
