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
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Configuration;
using Ninject;
using jarvis.addins.serverActions;
using jarvis.common.dtos.Actionhandling;
using jarvis.server.configuration;
using jarvis.server.repositories;

namespace jarvis.server.model.ActionHandling
{
    public interface IActionLogic
    {
        ActionResultDto Execute(ActionDto actionDto);
        void LoadServerActions();
    }

    public class ActionLogic : IActionLogic
    {
        private readonly IActionRegistry _actionRegistry;
        private readonly Func<IKernel> _kernel;
        private readonly IDatabaseManager _databaseManager;
        private readonly INHibernateConfiguration _nHibernateConfiguration;
        private List<Assembly> _addins = new List<Assembly>();

        public ActionLogic(IActionRegistry actionRegistry, Func<IKernel> kernel, IDatabaseManager databaseManager, INHibernateConfiguration  nHibernateConfiguration )
        {
            _actionRegistry = actionRegistry;
            _kernel = kernel;
            _databaseManager = databaseManager;
            _nHibernateConfiguration = nHibernateConfiguration;

            LoadServerActions();
        }

        public ActionResultDto Execute(ActionDto actionDto)
        {
            var serverAction = _actionRegistry.GetActionHandler(actionDto.ActionGroup);

            if (serverAction.CanExecute(actionDto))
                return serverAction.Execute(actionDto);

            return null;
        }

        public void LoadServerActions()
        {
            if (_addins.Any())
                return;

            AddAddInConfigHandling();
            LoadAddins();

            foreach (var addin in _addins)
            {

                var actionHandlerTypes = GetAddinTypes(addin, typeof(ServerAction));

                if (actionHandlerTypes.Any())
                    _nHibernateConfiguration.AddAssembly(addin);

                foreach (var triggerType in actionHandlerTypes)
                {
                    var actionHandler = (ServerAction)Activator.CreateInstance(triggerType);
                    _kernel().Inject(actionHandler);

                    actionHandler.Init(_kernel);

                    _actionRegistry.RegisterActionHandler(actionHandler);
                }
            }

            _nHibernateConfiguration.RecreateSessionFactory();
            _databaseManager.UpdateSchema();
        }

        private void AddAddInConfigHandling()
        {
            AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs e)
            {
                var requestedName = new AssemblyName(e.Name);

                var addinAssembly = _addins.Where(a => a.GetName().Name == requestedName.Name).SingleOrDefault();
                return addinAssembly;
            };
        }

        protected static IEnumerable<Type> GetAddinTypes(Assembly addin, Type addinType)
        {
            return addin.GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(addinType));
        }

        private void LoadAddins()
        {
            var addinFiles = Directory.GetFiles(WebConfigurationManager.AppSettings["AddinPath"]).Where(f => f.EndsWith(".dll"));

            foreach (var addinFile in addinFiles)
            {
                var assembly = Assembly.LoadFile(addinFile);
                _addins.Add(assembly);
            }
        }

    }
}