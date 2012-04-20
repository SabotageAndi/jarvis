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
using System.Linq;
using System.Reflection;
using Autofac;
using jarvis.addins.actions;
using jarvis.addins.trigger;
using jarvis.client.common;
using jarvis.client.common.ServiceClients;

namespace jarvis.client
{
    public class ActionTriggerClient : Client
    {
        private readonly IActionRegistry _actionRegistry;
        private readonly IActionServiceHost _actionServiceHost;

        public ActionTriggerClient(IClientService clientService, IConfiguration configuration, IServerStatusService serverStatusService,
                                   IActionServiceHost actionServiceHost, IActionRegistry actionRegistry)
            : base(clientService, configuration, serverStatusService)
        {
            _actionServiceHost = actionServiceHost;
            _actionRegistry = actionRegistry;
            Triggers = new List<Trigger>();
        }

        private List<Trigger> Triggers { get; set; }

        public override void Init(IContainer container)
        {
            base.Init(container);

            LoadTriggers();
            LoadActionHandlers();


            _actionServiceHost.Start();
        }

        private void LoadActionHandlers()
        {
            foreach (var addin in Addins)
            {
                var actionHandlerTypes = GetAddinTypes(addin, typeof (ActionHandler));
                foreach (var triggerType in actionHandlerTypes)
                {
                    var actionHandler = (ActionHandler) Activator.CreateInstance(triggerType);
                    _container.InjectProperties(actionHandler);

                    _actionRegistry.RegisterActionHandler(actionHandler);
                }
            }
        }

        private void LoadTriggers()
        {
            foreach (var addin in Addins)
            {
                var triggerTypes = GetAddinTypes(addin, typeof (Trigger));
                foreach (var triggerType in triggerTypes)
                {
                    var trigger = (Trigger) Activator.CreateInstance(triggerType);
                    _container.InjectProperties(trigger);
                    Triggers.Add(trigger);
                }
            }

            foreach (var trigger in Triggers)
            {
                trigger.init();
            }
        }

        public override void Shutdown()
        {
            foreach (var trigger in Triggers)
            {
                trigger.deinit();
            }

            base.Shutdown();
        }

        public override void Run()
        {
            foreach (var trigger in Triggers)
            {
                trigger.run();
            }

            base.Run();
        }
    }
}