using System.Runtime.Serialization;
using jarvis.common.dtos.Actionhandling;

namespace jarvis.common.dtos.Requests
{
    [DataContract]
    public class ActionExecuteRequest : Request
    {
        [DataMember]
        public ActionDto ActionDto { get; set; }
    }
}