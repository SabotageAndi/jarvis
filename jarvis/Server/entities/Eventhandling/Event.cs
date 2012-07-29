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
using jarvis.common.domain;
using jarvis.server.entities.Helper;
using jarvis.server.entities.Management;

namespace jarvis.server.entities.Eventhandling
{
    public class Event : Entity
    {
        public virtual string EventGroupType { get; set; }
        public virtual string EventType { get; set; }
        public virtual DateTime TriggeredDate { get; set; }
        public virtual DateTime? ProcessedDate { get; set; }
        public virtual Client Client { get; set; }
        public virtual string Data { get; set; }
    }

    public class EventMap : ClassMap<Event>
    {
        public EventMap()
        {
            Id(x => x.Id);
            References(x => x.Client);
            Map(x => x.EventGroupType);
            Map(x => x.EventType);
            Map(x => x.TriggeredDate);
            Map(x => x.ProcessedDate);
            Map(x => x.Data);
        }
    }
}