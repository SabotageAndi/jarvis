using jarvis.common.dtos.Management;

namespace jarvis.common.dtos.Requests
{
    public class LoginClientRequest : Request
    {
        public ClientDto ClientDto { get; set; }
    }
}