﻿// J.A.R.V.I.S. - Just a really versatile intelligent system
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

using FluentNHibernate.Mapping;

namespace jarvis.server.entities
{
    public class NextWorkflowStep : Entity
    {
        public virtual WorkflowStep NextStep { get; set; }
        public virtual WorkflowStep PreviousStep { get; set; }
    }

    public class NextWorkflowStepMap : ClassMap<NextWorkflowStep>
    {
        public NextWorkflowStepMap()
        {
            Id(x => x.Id).CustomSqlType("Serial").GeneratedBy.Native();
            References(x => x.NextStep);
            References(x => x.PreviousStep);
        }
    }
}