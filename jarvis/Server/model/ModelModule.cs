﻿// J.A.R.V.I.S. - Just A Rather Very Intelligent System
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

using Ninject.Modules;
using jarvis.server.model.ActionHandling;
using jarvis.server.repositories;

namespace jarvis.server.model
{
    public class ModelModule : NinjectModule
    {
        public override void Load()
        {

            Bind<IEventLogic>().To<EventLogic>().InTransientScope();
            Bind<IUserLogic>().To<UserLogic>().InTransientScope();
            Bind<IEventHandlingLogic>().To<EventHandlingLogic>().InTransientScope();
            Bind<IWorkflowLogic>().To<WorkflowLogic>().InTransientScope();
            Bind<IClientLogic>().To<ClientLogic>().InTransientScope();
            Bind<IActionLogic>().To<ActionLogic>().InTransientScope();
            Bind<IActionRegistry>().To<ActionRegistry>().InSingletonScope();
        }
    }
}