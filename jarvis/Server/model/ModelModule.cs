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

using Autofac;
using jarvis.server.repositories;

namespace jarvis.server.model
{
    public class ModelModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new RepositoryModule());

            builder.RegisterType<EventLogic>().As<IEventLogic>().InstancePerDependency();
            builder.RegisterType<UserLogic>().As<IUserLogic>().InstancePerDependency();
            builder.RegisterType<EventHandlingLogic>().As<IEventHandlingLogic>().InstancePerDependency();
            builder.RegisterType<WorkflowLogic>().As<IWorkflowLogic>().InstancePerDependency();
            builder.RegisterType<ClientLogic>().As<IClientLogic>().InstancePerDependency();
            builder.RegisterType<ActionLogic>().As<IActionLogic>().InstancePerDependency();

            base.Load(builder);
        }
    }
}