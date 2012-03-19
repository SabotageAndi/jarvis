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

using System;
using System.Runtime.Serialization;
using jarvis.common.domain;

namespace jarvis.common.dtos
{
    [DataContract]
    public class EventDto
    {
        [DataMember]
        public DateTime TriggeredDate { get; set; }

        [DataMember]
        public EventGroupTypes EventGroupTypes { get; set; }

        [DataMember]
        public EventType EventType { get; set; }

        [DataMember]
        public String Data { get; set; }
    }
}