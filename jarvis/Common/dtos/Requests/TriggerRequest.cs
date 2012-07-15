using System.Runtime.Serialization;
using jarvis.common.dtos.Eventhandling;

namespace jarvis.common.dtos.Requests
{
    [DataContract]
    public class TriggerRequest : Request
    {
        [DataMember]
        public EventDto EventDto { get; set; }

    }
}