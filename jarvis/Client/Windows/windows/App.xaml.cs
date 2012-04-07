using System.Threading;
using System.Windows;
using Autofac;
using jarvis.client.common;

namespace jarvis.client.windows
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static IContainer _container;

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

            client.Init();

            var window = new MainWindow();
            window.Show();

            client.Run();
        }

    }
}
