using jarvis.common.dtos.Management;

namespace jarvis.common.dtos.Requests
{
    public class RegisterClientRequest : Request
    {
        public ClientDto ClientDto { get; set; }
    }
}