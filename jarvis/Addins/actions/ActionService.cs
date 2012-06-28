// J.A.R.V.I.S. - Just A Rather Very Intelligent System
// Copyright (C) 2012 Andreas Willich <sabotageandi@gmail.com>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using ServiceStack.ServiceInterface;
using jarvis.common.dtos;
using jarvis.common.dtos.Actionhandling;
using jarvis.common.dtos.Requests;
using log4net;

namespace jarvis.addins.actions
{
    public class ActionService : ServiceBase<ActionExecuteRequest>
    {
        private readonly IActionRegistry _actionRegistry;
        private readonly ILog _log;
        private bool _isExecuting;

        public ActionService(IActionRegistry actionRegistry, ILog log)
        {
            _actionRegistry = actionRegistry;
            _log = log;
            _isExecuting = false;
        }

        protected override object HandleException(ActionExecuteRequest request, Exception ex)
        {
            _log.ErrorFormat("Error at executing action {0}.{1}: {2}", request.ActionDto.ActionGroup, request.ActionDto.Action, ex);
            return base.HandleException(request, ex);
        }

        protected override object Run(ActionExecuteRequest request)
        {
            _isExecuting = true;
            try
            {
                var actionHandler = _actionRegistry.GetActionHandler(request.ActionDto.ActionGroup);

                if (!actionHandler.CanHandleAction(request.ActionDto))
                {
                    return null;
                }

                var actionResultDto = actionHandler.DoAction(request.ActionDto);
                return new ResultDto<ActionResultDto>(actionResultDto);
            }
            finally
            {
                _isExecuting = false;
            }
        }
    }
}