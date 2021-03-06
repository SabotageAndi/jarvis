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
using FluentNHibernate.Mapping;
using jarvis.server.entities.Eventhandling;
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
        public virtual Event Event { get; set; }
    }

    public class WorkflowQuereMap : ClassMap<WorkflowQueue>
    {
        public WorkflowQuereMap()
        {
            Id(x => x.Id);
            Map(x => x.QueueDate);
            Map(x => x.StartDate);
            Map(x => x.EndDate);
            References(x => x.DefinedWorkflow);
            References(x => x.RunnedWorkflow);
            References(x => x.Event);
        }
    }
}