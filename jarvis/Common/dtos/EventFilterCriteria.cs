using System;
using System.Runtime.Serialization;

namespace jarvis.common.dtos
{
    [DataContract]
    public class EventFilterCriteria
    {
        [DataMember]
        public DateTime? MaxTriggeredDate { get; set; }

        [DataMember]
        public DateTime? MinTriggeredDate { get; set; }
    }
}