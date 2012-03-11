using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using jarvis.common.dtos;
using jarvis.server.model;

namespace jarvis.server.web.services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TriggerService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TriggerService.svc or TriggerService.svc.cs at the Solution Explorer and start debugging.
    public class TriggerService : ITriggerService
    {
        private readonly ITriggerLogic _triggerLogic;

        public TriggerService(ITriggerLogic triggerLogic)
        {
            _triggerLogic = triggerLogic;
        }

        public void EventHappend(EventDto eventDto)
        {
            _triggerLogic.eventRaised(eventDto);
        }
    }
}
