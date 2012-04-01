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

using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using jarvis.server.entities;

namespace jarvis.server.configuration
{
    public interface INHibernateConfiguration
    {
        ISessionFactory GetSessionFactory();
        FluentConfiguration GetConfiguration();
    }

    public class PostgreSqlConfiguration : INHibernateConfiguration
    {
        private FluentConfiguration _configuration;

        public ISessionFactory GetSessionFactory()
        {
            return GetConfiguration().BuildSessionFactory();
        }

        public FluentConfiguration GetConfiguration()
        {
            if (_configuration != null)
            {
                return _configuration;
            }

            _configuration = Fluently.Configure()
                .Database(PostgreSQLConfiguration.Standard.ConnectionString(
                    builder =>
                    builder.Database("jarvis").Host("localhost").Username("jarvis").Password("jarvis").Port(5432)))
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<Entity>());

            return _configuration;
        }
    }
}