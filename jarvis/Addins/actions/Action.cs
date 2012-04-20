using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace jarvis.addins.actions
{
    public abstract class Action
    {
        public client.common.ServiceClients.IActionService ActionService { get; set; }

        public abstract string PropertyName { get; }
    }
}
