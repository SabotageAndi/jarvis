using jarvis.common.dtos.Management;

namespace jarvis.common.dtos.Requests
{
    public class LogoffClientRequest : Request
    {
        public ClientDto ClientDto { get; set; }
    }
}