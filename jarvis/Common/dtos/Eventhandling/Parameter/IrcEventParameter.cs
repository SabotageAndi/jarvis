using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace jarvis.common.dtos.Eventhandling.Parameter
{
    public class IrcEventParameter : EventParameterDto
    {
        public string Channels { get; set; }
        public string User { get; set; }
        public string Message { get; set; }
    }
}
