using System;

namespace jarvis.common.dtos.Requests
{
    public class GetAllEventsSinceRequest : Request
    {
        public String Ticks { get; set; }    
    }
}