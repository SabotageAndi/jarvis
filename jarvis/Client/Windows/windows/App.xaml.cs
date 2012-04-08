using System.Drawing;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using Autofac;
using jarvis.client.common;
using Application = System.Windows.Application;

namespace jarvis.client.windows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IContainer _container;
        private NotifyIcon _systray; 

        static void Bootstrap()
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.RegisterModule<CommonModule>();
            containerBuilder.RegisterType<ConfigFileConfiguration>().As<IConfiguration>().SingleInstance();

            _container = containerBuilder.Build();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            Bootstrap();

            var client = _container.Resolve<Client>();

            //TODO check if server online
            Thread.Sleep(30000);

            client.Init(_container);
            client.OnShutDown += client_OnShutDown;

            InitSystray();
            client.Run();
            
        }

        void client_OnShutDown()
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
