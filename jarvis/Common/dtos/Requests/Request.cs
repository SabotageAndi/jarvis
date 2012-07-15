using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ServiceStack.ServiceInterface;

namespace jarvis.common.dtos.Requests
{
    [DataContract]
    public abstract class Request
    {
        [DataMember]
        public string Version { get; set; }
    }
}
