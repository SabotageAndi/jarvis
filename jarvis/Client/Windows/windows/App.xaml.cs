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
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using Ninject;
using jarvis.addins.actions;
using jarvis.client.common;
using jarvis.client.common.ServiceClients;
using log4net;
using log4net.Config;
using Application = System.Windows.Application;

namespace jarvis.client.windows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IKernel _container;
        private NotifyIcon _systray;

        private static void Bootstrap()
        {

            XmlConfigurator.Configure();
            var log = LogManager.GetLogger("windowsclient");

            var containerBuilder = new StandardKernel();

            containerBuilder.Load(new CommonModule(), new ActionModule(), new ClientModule(), new ServiceClientModule());
            containerBuilder.Bind<Client>().To<ActionTriggerClient>().InSingletonScope();
            containerBuilder.Bind<Func<IKernel>>().ToMethod(ctx => () => Client.Container);
            containerBuilder.Bind<ILog>().ToConstant(log).InSingletonScope();

            _container = containerBuilder;
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                Bootstrap();

                var client = _container.Get<Client>();


                client.Init(_container);
                client.OnShutDown += client_OnShutDown;

                InitSystray();
                client.Run();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
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
    }
}