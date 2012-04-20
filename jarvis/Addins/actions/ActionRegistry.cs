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
using System.Collections.Generic;
using jarvis.common.domain;

namespace jarvis.addins.actions
{
    public interface IActionRegistry
    {
        void RegisterActionHandler(IActionHandler actionHandler);
        IActionHandler GetActionHandler(ActionGroup actionGroup);
    }

    public class ActionRegistry : IActionRegistry
    {
        public ActionRegistry()
        {
            ActionHandlers = new Dictionary<ActionGroup, IActionHandler>();
        }

        private Dictionary<ActionGroup, IActionHandler> ActionHandlers { get; set; }

        public void RegisterActionHandler(IActionHandler actionHandler)
        {
            if (ActionHandlers.ContainsKey(actionHandler.ActionGroup))
            {
                throw new ActionHandlerAlreadyRegisteredException();
            }

            ActionHandlers.Add(actionHandler.ActionGroup, actionHandler);
        }

        public IActionHandler GetActionHandler(ActionGroup actionGroup)
        {
            if (!ActionHandlers.ContainsKey(actionGroup))
            {
                throw new ActionHandlerNotFoundException();
            }

            return ActionHandlers[actionGroup];
        }
    }

    public class ActionHandlerNotFoundException : Exception
    {
    }

    public class ActionHandlerAlreadyRegisteredException : Exception
    {
    }
}