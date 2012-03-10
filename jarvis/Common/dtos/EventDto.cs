using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
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
    }
}
