using System.Runtime.Serialization;
using jarvis.common.dtos.Management;

namespace jarvis.common.dtos.Requests
{
    [DataContract]
    public class RegisterClientRequest : Request
    {
        [DataMember]
        public ClientDto ClientDto { get; set; }

    }
}