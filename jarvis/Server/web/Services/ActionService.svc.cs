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
using jarvis.server.model;

namespace jarvis.server.web.services
{
    public class ActionService : IActionService
    {
        private readonly IActionLogic _actionLogic;

        public ActionService(IActionLogic actionLogic)
        {
            _actionLogic = actionLogic;
        }

        public ActionResultDto Execute(ActionDto actionDto)
        {
            return _actionLogic.Execute(actionDto);
        }
    }
}