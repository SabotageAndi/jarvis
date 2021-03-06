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

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Configuration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using NHibernate;
using jarvis.server.entities;

namespace jarvis.server.configuration
{
    public class DatabaseConfiguration : INHibernateConfiguration
    {
        private FluentConfiguration _configuration;
        private ISessionFactory _sessionFactory;
        private readonly List<Assembly> _assemblies;

        public DatabaseConfiguration()
        {
            _assemblies = new List<Assembly>();

            RecreateSessionFactory();
        }

        internal DatabaseConfigurationSection JarvisClientConfigurationSection
        {
            get { return WebConfigurationManager.GetSection(DatabaseConfigurationSection.SectionName) as DatabaseConfigurationSection; }
        }


        public ISessionFactory GetSessionFactory()
        {
            return _sessionFactory;
        }

        public FluentConfiguration GetConfiguration()
        {
            return _configuration;
        }

        public void AddAssembly(Assembly assembly)
        {
            _assemblies.Add(assembly);
        }

        public void RecreateSessionFactory()
        {
            _configuration = GetFluentConfiguration();
            _sessionFactory = _configuration.BuildSessionFactory();
        }

        private FluentConfiguration GetFluentConfiguration()
        {
            return Fluently.Configure()
                .Database(GetDbConfiguration())
                .Mappings(m =>
                              {
                                  foreach (var convention in GetGeneralConvertion())
                                  {
                                      m.FluentMappings.Conventions.Add(convention);
                                  }

                                  foreach (var convention in GetDbSpecificConvention())
                                  {
                                      m.FluentMappings.Conventions.Add(convention);
                                  }                                  m.FluentMappings.AddFromAssemblyOf<Entity>();

                                  foreach (var assembly in _assemblies)
                                  {
                                      m.FluentMappings.Conventions.AddAssembly(assembly);
                                      m.FluentMappings.AddFromAssembly(assembly);
                                  }
                              });
        }


        private IEnumerable<IConvention> GetGeneralConvertion()
        {
            return new List<IConvention>()
                       {
                           new EnumConvention()
                       };
        }

        private IEnumerable<IConvention> GetDbSpecificConvention()
        {
            switch (JarvisClientConfigurationSection.Type.Value.ToUpper())
            {
                case "POSTGRESQL":
                    return new List<IConvention>()
                               {
                                    new PostgreSqlPrimaryKeyConvention()   
                               };
                case "MSSQL":
                    return new List<IConvention>()
                               {
                                   new MsSqlPrimaryKeyConvention()
                               };
            }

            throw new Exception("Database Type not found");
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