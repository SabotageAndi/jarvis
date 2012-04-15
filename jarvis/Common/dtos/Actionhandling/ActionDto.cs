using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jarvis.common.domain;
using jarvis.common.dtos.Management;
using Action = jarvis.common.domain.Action;

namespace jarvis.common.dtos.Actionhandling
{
    public class ActionDto
    {
        public ActionGroup ActionGroup { get; set; }
        public Action Action { get; set; }
        public List<ParameterDto> Parameters { get; set; }
        public ClientDto Client { get; set; }
    }
}
