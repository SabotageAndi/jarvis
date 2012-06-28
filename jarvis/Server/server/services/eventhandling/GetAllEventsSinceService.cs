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
    class GetAllEventsSinceService : ServiceBase<GetAllEventsSinceRequest>
    {
        private readonly ITransactionProvider _transactionProvider;
        private readonly IEventLogic _eventLogic;
        private readonly ILog _log;

        public GetAllEventsSinceService(ITransactionProvider transactionProvider, IEventLogic eventLogic, ILog log)
        {
            _transactionProvider = transactionProvider;
            _eventLogic = eventLogic;
            _log = log;
        }

        protected override object HandleException(GetAllEventsSinceRequest request, Exception ex)
        {
            _log.ErrorFormat("Error at getting event list: {0}", ex);
            return base.HandleException(request, ex);
        }

        protected override object Run(GetAllEventsSinceRequest request)
        {
            using (var transactionScope = _transactionProvider.StartReadWriteTransaction())
            {
                var date = new DateTime(Convert.ToInt64(request.Ticks));

                var allEventsSince = _eventLogic.GetAllEventsSince(transactionScope, date);

                transactionScope.Commit();
                return new ResultDto<List<EventDto>>(allEventsSince);
            }
        }
    }
}
