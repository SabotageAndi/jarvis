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
using System.Configuration;
using System.Web.Configuration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using jarvis.server.entities;

namespace jarvis.server.configuration
{
    public class DatabaseConfiguration : INHibernateConfiguration
    {
        private readonly FluentConfiguration _configuration;
        private readonly ISessionFactory _sessionFactory;
        private Configuration _configFileConfiguration;

        public DatabaseConfiguration()
        {
            _configuration = GetFluentConfiguration();
            _sessionFactory = _configuration.BuildSessionFactory();
        }

        internal DatabaseConfigurationSection JarvisClientConfigurationSection
        {
            get { return WebConfigurationManager.GetSection(DatabaseConfigurationSection.SectionName) as DatabaseConfigurationSection; }
        }

//        private System.Configuration.Configuration Configuration
//        {
//            get
//            {
//                if (_configuration == null)
//                {
//#if DEBUG
//                    string applicationName = Environment.GetCommandLineArgs()[0];
//#else
//                    string applicationName = Environment.GetCommandLineArgs()[0]+ ".exe";
//#endif

//                    string exePath = System.IO.Path.Combine(Environment.CurrentDirectory, applicationName);

//                    _configFileConfiguration = WebConfigurationManager.
//                }

//                return _configFileConfiguration;
//            }
//        }

        public ISessionFactory GetSessionFactory()
        {
            return _sessionFactory;
        }

        public FluentConfiguration GetConfiguration()
        {
            return _configuration;
        }

        private FluentConfiguration GetFluentConfiguration()
        {
            return Fluently.Configure()
                .Database(GetDbConfiguration())
                .Mappings(m =>
                              {
                                  m.FluentMappings.AddFromAssemblyOf<Entity>();
                                  m.FluentMappings.Conventions.AddFromAssemblyOf<Entity>();
                              });
        }

        private IPersistenceConfigurer GetDbConfiguration()
        {
            switch (JarvisClientConfigurationSection.Type.Value.ToUpper())
            {
                case "POSTGRESQL":
                    return GetPostgreSqlConfigurer();
                case "MSSQL":
                    return GetMSSqlConfigurer();
            }

            throw new Exception("Database Type not found");
        }

        private IPersistenceConfigurer GetMSSqlConfigurer()
        {
            return MsSqlConfiguration.MsSql2008.ConnectionString(
                builder =>
                builder.Database(JarvisClientConfigurationSection.Database.Value)
                       .Server(JarvisClientConfigurationSection.Host.Value)
                       .Username(JarvisClientConfigurationSection.User.Value)
                       .Password(JarvisClientConfigurationSection.Password.Value));
        }

        private IPersistenceConfigurer GetPostgreSqlConfigurer()
        {
            return PostgreSQLConfiguration.Standard.ConnectionString(
                builder =>
                builder.Database(JarvisClientConfigurationSection.Database.Value)
                       .Host(JarvisClientConfigurationSection.Host.Value)
                       .Username(JarvisClientConfigurationSection.User.Value)
                       .Password(JarvisClientConfigurationSection.Password.Value)
                       .Port(5432));
        }
    }
}