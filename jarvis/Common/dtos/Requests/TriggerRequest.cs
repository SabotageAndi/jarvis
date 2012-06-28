using jarvis.common.dtos.Eventhandling;

namespace jarvis.common.dtos.Requests
{
    public class TriggerRequest : Request
    {
        public EventDto EventDto { get; set; }
    }
}