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
using jarvis.common.dtos;
using jarvis.common.dtos.Actionhandling;

namespace jarvis.addins.actions
{
    [ServiceContract]
    public interface IActionService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "execute", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        ActionResultDto Execute(ActionDto actionDto);

        [OperationContract]
        [WebGet(UriTemplate = "isExecuting", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ResultDto<Boolean> IsExecuting();
    }


    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, AddressFilterMode=AddressFilterMode.Any)]
    public class ActionService : IActionService
    {
        private readonly IActionRegistry _actionRegistry;
        private bool _isExecuting;

        public ActionService(IActionRegistry actionRegistry)
        {
            _actionRegistry = actionRegistry;
            _isExecuting = false;
        }

        public ActionResultDto Execute(ActionDto actionDto)
        {
            _isExecuting = true;
            try
            {
                var actionHandler = _actionRegistry.GetActionHandler(actionDto.ActionGroup);

                if (!actionHandler.CanHandleAction(actionDto))
                {
                    return null;
                }

                return actionHandler.DoAction(actionDto);
            }
            finally
            {
                _isExecuting = false;
            }
        }

        public ResultDto<bool> IsExecuting()
        {
            return new ResultDto<bool>(_isExecuting);
        }
    }
}