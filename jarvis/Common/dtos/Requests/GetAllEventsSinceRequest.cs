using System;
using System.Runtime.Serialization;

namespace jarvis.common.dtos.Requests
{
    [DataContract]
    public class GetAllEventsSinceRequest : Request
    {
        [DataMember]
        public String Ticks { get; set; }

    }
}