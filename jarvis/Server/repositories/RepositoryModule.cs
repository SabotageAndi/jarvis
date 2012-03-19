// J.A.R.V.I.S. - Just a really versatile intelligent system
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
using jarvis.server.repositories.Infrastructure;

namespace jarvis.server.repositories
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new ConfigurationModule());

            builder.RegisterType<TransactionFactory>().As<ITransactionFactory>().InstancePerLifetimeScope();
            builder.RegisterType<SessionFactory>().As<ISessionFactory>().InstancePerLifetimeScope();

            builder.RegisterType<TriggerRepository>().As<ITriggerRepository>().InstancePerLifetimeScope();
            builder.RegisterType<UserRepository>().As<IUserRepository>().InstancePerLifetimeScope();

            base.Load(builder);
        }
    }
}