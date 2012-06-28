using jarvis.common.dtos.Actionhandling;

namespace jarvis.common.dtos.Requests
{
    public class ActionExecuteRequest : Request
    {
        public ActionDto ActionDto { get; set; }
    }
}