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
using System.Runtime.Serialization;
using jarvis.common.domain;

namespace jarvis.common.dtos.Eventhandling
{
    [DataContract]
    public class EventDto
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public DateTime TriggeredDate { get; set; }

        public EventGroupTypes EventGroupTypes
        {
            get { return (EventGroupTypes) EventGroupTypesValue; }
            set { EventGroupTypesValue = (int) value; }
        }

        public EventType EventType
        {
            get { return (EventType) EventTypeValue; }
            set { EventTypeValue = (int) value; }
        }

        [DataMember]
        public int EventGroupTypesValue { get; set; }

        [DataMember]
        public int EventTypeValue { get; set; }

        [DataMember]
        public String Data { get; set; }
    }
}