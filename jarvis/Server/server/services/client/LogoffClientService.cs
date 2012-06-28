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

namespace jarvis.server.services.client
{
    class LogoffClientService : ServiceBase<LogoffClientRequest>
    {
        private readonly IClientLogic _clientLogic;
        private readonly ITransactionProvider _transactionProvider;
        private readonly ILog _log;

        public LogoffClientService(IClientLogic clientLogic, ITransactionProvider transactionProvider, ILog log)
        {
            _clientLogic = clientLogic;
            _transactionProvider = transactionProvider;
            _log = log;
        }

        protected override object HandleException(LogoffClientRequest request, Exception ex)
        {
            _log.ErrorFormat("Error at logoff of client {0}: {1}", request.ClientDto.Name, ex);

            return base.HandleException(request, ex);
        }

        protected override object Run(LogoffClientRequest request)
        {
            using (var transactionScope = _transactionProvider.StartReadWriteTransaction())
            {
                _clientLogic.Logoff(transactionScope, request.ClientDto);
                transactionScope.Commit();

                return new ResultDto();
            }
        }
    }
}
