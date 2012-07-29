using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceInterface;
using jarvis.common.dtos;
using jarvis.common.dtos.Eventhandling;
using jarvis.common.dtos.Requests;
using jarvis.server.common.Database;
using jarvis.server.model;
using log4net;

namespace jarvis.server.services.eventhandling
{
    class GetEventToProcessService : ServiceBase<GetEventsToProcess>
    {
        private readonly ITransactionProvider _transactionProvider;
        private readonly IEventLogic _eventLogic;
        private readonly ILog _log;

        public GetEventToProcessService(ITransactionProvider transactionProvider, IEventLogic eventLogic, ILog log)
        {
            _transactionProvider = transactionProvider;
            _eventLogic = eventLogic;
            _log = log;
        }

        protected override object HandleException(GetEventsToProcess request, Exception ex)
        {
            _log.ErrorFormat("Error at getting event list: {0}", ex);
            return base.HandleException(request, ex);
        }

        protected override object Run(GetEventsToProcess request)
        {
            using (var transactionScope = _transactionProvider.StartReadWriteTransaction())
            {
                var eventsToProcess = _eventLogic.GetEventsToProcess(transactionScope);

                transactionScope.Commit();
                return new ResultDto<EventDto>(eventsToProcess);
            }
        }
    }
}
