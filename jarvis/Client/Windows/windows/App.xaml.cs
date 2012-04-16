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
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using Autofac;
using jarvis.client.common;
using jarvis.client.common.Actions;
using jarvis.client.common.Actions.ActionHandlers;
using jarvis.client.common.ServiceClients;
using jarvis.client.common.Triggers.FileSystemTrigger;
using Application = System.Windows.Application;
using Trigger = jarvis.client.common.Triggers.Trigger;

namespace jarvis.client.windows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IContainer _container;
        private NotifyIcon _systray;

        private static void Bootstrap()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<CommonModule>();
            containerBuilder.RegisterType<ConfigFileConfiguration>().As<IConfiguration>().SingleInstance();
            containerBuilder.RegisterType<ActionTriggerClient>().SingleInstance();

            _container = containerBuilder.Build();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Bootstrap();

            var client = _container.Resolve<ActionTriggerClient>();


            client.Init(_container);
            client.OnShutDown += client_OnShutDown;

            InitSystray();
            client.Run();
        }

        private void client_OnShutDown()
        {
            App.Current.Shutdown();
        }

        private void InitSystray()
        {
            _systray = new NotifyIcon
                           {
                               Icon = new Icon("app.ico"),
                               Text = "J.A.R.V.I.S. - Just A Rather Very Intelligent System"
                           };

            var contextMenu = new ContextMenu();

            var exitMenuItem = new MenuItem("Exit", (sender, args) => Client.Current.Shutdown());
            contextMenu.MenuItems.Add(exitMenuItem);

            _systray.ContextMenu = contextMenu;
            _systray.Visible = true;
        }


        public class ActionTriggerClient : Client
        {
            private readonly IActionServiceHost _actionServiceHost;
            private readonly IActionRegistry _actionRegistry;

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

                _actionRegistry.RegisterActionHandler(new FileActionHandler());

                _actionServiceHost.Start();

                Triggers.Add(_container.Resolve<FileSystemTrigger>());

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
}