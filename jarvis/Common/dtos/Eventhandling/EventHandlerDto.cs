using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jarvis.common.domain;

namespace jarvis.common.dtos.Eventhandling
{
    public class EventHandlerDto
    {
        public int Id { get; set; }
        public EventGroupTypes? EventGroupTypes { get; set; }
        public EventType? EventType { get; set; }
        public int DefinedWorkflowId { get; set; }
    }
}
