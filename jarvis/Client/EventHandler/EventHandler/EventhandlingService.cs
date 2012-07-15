using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ServiceStack.ServiceInterface;
using jarvis.client.common.ServiceClients;
using jarvis.common.dtos;
using jarvis.common.dtos.Eventhandling;
using jarvis.common.dtos.Requests;
using jarvis.common.dtos.Workflow;
using log4net;

namespace jarvis.client.eventhandler
{
    class EventhandlingService : ServiceBase<EventhandlingTriggerRequest>
    {
        private readonly IEventhandlingLockManager _eventhandlingLockManager;
        private readonly ILog _log;

        public EventhandlingService(IEventhandlingLockManager eventhandlingLockManager, ILog log)
        {
            _eventhandlingLockManager = eventhandlingLockManager;
            _log = log;
        }

        protected override object HandleException(EventhandlingTriggerRequest request, Exception ex)
        {
            _log.FatalFormat("Error at executing action: {0}", ex);
            return base.HandleException(request, ex);
        }

        protected override object Run(EventhandlingTriggerRequest request)
        {
            _eventhandlingLockManager.Release();
            _log.Info("Event triggered");

            return new ResultDto();
        }


    }
}
