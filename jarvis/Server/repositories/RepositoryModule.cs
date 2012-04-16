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
using jarvis.server.configuration;

namespace jarvis.server.repositories
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new ConfigurationModule());

            builder.RegisterType<EventRepository>().As<IEventRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();
            builder.RegisterType<EventHandlerRepository>().As<IEventHandlerRepository>().InstancePerLifetimeScope();
            builder.RegisterType<WorkflowQueueRepository>().As<IWorkflowQueueRepository>().InstancePerLifetimeScope();
            builder.RegisterType<DefinedWorkflowRepository>().As<IDefinedWorkflowRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RunnedWorkflowRepository>().As<IRunnedWorkflowRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RunnedWorkflowStepRepository>().As<IRunnedWorkflowStepRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RunnedTaskRespository>().As<IRunnedTaskRepository>().InstancePerLifetimeScope();
            builder.RegisterType<RunnedNextWorkflowStepRepository>().As<IRunnedNextWorkflowStepRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ClientRepository>().As<IClientRepository>().InstancePerLifetimeScope();
            builder.RegisterType<ParameterRepository>().As<IParameterRepository>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}