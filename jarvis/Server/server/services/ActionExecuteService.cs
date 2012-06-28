using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceInterface;
using jarvis.common.dtos;
using jarvis.common.dtos.Actionhandling;
using jarvis.common.dtos.Requests;
using jarvis.server.common.Database;
using jarvis.server.model.ActionHandling;
using log4net;

namespace jarvis.server.services
{
    class ActionExecuteService : ServiceBase<ActionExecuteRequest>
    {
        private readonly IActionLogic _actionLogic;
        private readonly ITransactionProvider _transactionProvider;
        private readonly ILog _log;

        public ActionExecuteService(IActionLogic actionLogic, ITransactionProvider transactionProvider, ILog log)
        {
            _actionLogic = actionLogic;
            _transactionProvider = transactionProvider;
            _log = log;
        }

        protected override object HandleException(ActionExecuteRequest request, Exception ex)
        {
            _log.FatalFormat("Error at executing action: {0}", ex);
            return base.HandleException(request, ex);
        }

        protected override object Run(ActionExecuteRequest request)
        {
            using (var transactionScope = _transactionProvider.StartReadWriteTransaction())
            {
                var actionResultDto = _actionLogic.Execute(transactionScope, request.ActionDto);

                transactionScope.Commit();
                return new ResultDto<ActionResultDto>(actionResultDto);
            }
        }
    }
}
