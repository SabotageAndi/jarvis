using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using jarvis.common.domain;

namespace jarvis.server.web.Areas.Events.Models
{
    public class TriggeredEvent
    {
        
        public DateTime TriggeredDate { get; set; }

        public EventGroupTypes EventGroupTypes { get; set; }

        public EventType EventType { get; set; }

        public String Data { get; set; }
    }
}