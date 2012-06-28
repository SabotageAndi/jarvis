using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceInterface;
using jarvis.common.dtos;
using jarvis.common.dtos.Requests;
using jarvis.server.common.Database;
using jarvis.server.model;
using log4net;

namespace jarvis.server.services
{
    class TriggerService : ServiceBase<TriggerRequest>
    {
        private readonly IEventLogic _eventLogic;
        private readonly ITransactionProvider _transactionProvider;
        private readonly ILog _log;

        public TriggerService(IEventLogic eventLogic, ITransactionProvider transactionProvider, ILog log)
        {
            _eventLogic = eventLogic;
            _transactionProvider = transactionProvider;
            _log = log;
        }

        protected override object HandleException(TriggerRequest request, Exception ex)
        {
            _log.ErrorFormat("Error at triggering an event: {0}", ex);
            return base.HandleException(request, ex);
        }

        protected override object Run(TriggerRequest request)
        {
            using (var transactionScope = _transactionProvider.StartReadWriteTransaction())
            {
                _eventLogic.eventRaised(transactionScope, request.EventDto);
                transactionScope.Commit();

                return new ResultDto();
            }
        }
    }
}
