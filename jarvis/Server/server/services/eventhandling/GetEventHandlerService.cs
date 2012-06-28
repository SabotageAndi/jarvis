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
    class GetEventHandlerService : ServiceBase<GetEventHandlerRequest>
    {
        private readonly IEventHandlingLogic _eventHandlingLogic;
        private readonly ILog _log;
        private readonly ITransactionProvider _transactionProvider;

        public GetEventHandlerService(ITransactionProvider transactionProvider, IEventHandlingLogic eventHandlingLogic, ILog log)
        {
            _transactionProvider = transactionProvider;
            _eventHandlingLogic = eventHandlingLogic;
            _log = log;
        }

        protected override object HandleException(GetEventHandlerRequest request, Exception ex)
        {
            _log.ErrorFormat("Error at getting eventhandlers: {0}", ex);
            return base.HandleException(request, ex);
        }

        protected override object Run(GetEventHandlerRequest request)
        {
            using (var transactionScope = _transactionProvider.StartReadTransaction())
            {
                var eventHandlerDtos = _eventHandlingLogic.GetAllEventHandler(transactionScope);
                return new ResultDto<List<EventHandlerDto>>(eventHandlerDtos);
            }
        }
    }
}
