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

using jarvis.common.dtos.Actionhandling;
using jarvis.server.common.Database;
using jarvis.server.model;
using jarvis.server.model.ActionHandling;

namespace jarvis.server.web.services
{
    public class ActionService : IActionService
    {
        private readonly IActionLogic _actionLogic;
        private readonly ITransactionProvider _transactionProvider;

        public ActionService(IActionLogic actionLogic, ITransactionProvider transactionProvider)
        {
            _actionLogic = actionLogic;
            _transactionProvider = transactionProvider;
        }

        public ActionResultDto Execute(ActionDto actionDto)
        {
            using (_transactionProvider.StartReadWriteTransaction())
            {
                var actionResultDto = _actionLogic.Execute(actionDto);

                _transactionProvider.CurrentScope.Commit();
                return actionResultDto;
            }
        }
    }
}