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

using Ninject.Modules;

namespace jarvis.server.services
{
    public class ServiceModule : NinjectModule
    {
        public override void Load()
        {
            //Bind<TriggerService>().ToSelf().Named("jarvis.server.web.Services.TriggerService");
            //Bind<EventHandlingService>().ToSelf().Named("jarvis.server.web.services.EventHandlingService");
            //Bind<WorkflowService>().ToSelf().Named("jarvis.server.web.services.WorkflowService");
            //Bind<ClientService>().ToSelf().Named("jarvis.server.web.services.ClientService");
            //Bind<ServerStatusService>().ToSelf().Named("jarvis.server.web.services.ServerStatusService");
            //Bind<ActionService>().ToSelf().Named("jarvis.server.web.services.ActionService");

            Bind<TriggerService>().ToSelf().InSingletonScope();
            Bind<EventHandlingService>().ToSelf().InSingletonScope();
            Bind<WorkflowService>().ToSelf().InSingletonScope();
            Bind<ClientService>().ToSelf().InSingletonScope();
            Bind<ServerStatusService>().ToSelf().InSingletonScope();
            Bind<ActionService>().ToSelf().InSingletonScope();

            Bind<ITriggerService>().To<TriggerService>().InSingletonScope();
            Bind<IEventHandlingService>().To<EventHandlingService>().InSingletonScope();
            Bind<IWorkflowService>().To<WorkflowService>().InSingletonScope();
            Bind<IClientService>().To<ClientService>().InSingletonScope();
            Bind<IServerStatusService>().To<ServerStatusService>().InSingletonScope();
            Bind<IActionService>().To<ActionService>().InSingletonScope();
        }
    }
}