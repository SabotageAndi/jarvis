using System;
using System.Reflection;
using Funq;
using Ninject;
using ServiceStack.Logging.Support.Logging;
using ServiceStack.WebHost.Endpoints;
using jarvis.client.common;
using jarvis.common.dtos.Requests;
using jarvis.common.logic;
using log4net;

namespace jarvis.client.eventhandler
{
    internal interface IEventhandlingServiceHost
    {
        void Start();
        void Init();
    }

    class EventhandlingServiceHost : AppHostHttpListenerBase, IEventhandlingServiceHost
    {
        private readonly IKernel _kernel;
        private readonly IConfiguration _configuration;
        private readonly ILog _log;

        public EventhandlingServiceHost(IKernel kernel, IConfiguration configuration, ILog log)
            :base("JarvisWorker", Assembly.GetExecutingAssembly())
        {
            _kernel = kernel;
            _configuration = configuration;
            _log = log;
        }

        private String GetBaseAddress()
        {
            return String.Format("http://{0}:{1}/", "*", _configuration.LocalPort);
        }

        public override void Configure(Container container)
        {
            container.Adapter = new NinjectIocAdapter(_kernel);

            base.SetConfig(new EndpointHostConfig
            {
                GlobalResponseHeaders =
                {
                    { "Access-Control-Allow-Origin", "*" },
                    { "Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS" },
                },
                DebugMode = true,
                WsdlServiceNamespace = "http://www.servicestack.net/types",
            });

            this.ReceiveWebRequest += new ServiceStack.WebHost.Endpoints.Support.DelReceiveWebRequest(EventhandlingServiceHost_ReceiveWebRequest);

            Routes.Add<EventhandlingTriggerRequest>("/event/trigger", "POST");
            ServiceStack.Logging.LogManager.LogFactory = new ConsoleLogFactory();
        }

        void EventhandlingServiceHost_ReceiveWebRequest(System.Net.HttpListenerContext context)
        {
            _log.Debug("WebRequest received");
        }

        public void Start()
        {
            var baseAddress = GetBaseAddress();

            _log.InfoFormat("Starting EventhandlingServiceHost on {0}", baseAddress);

            this.Start(baseAddress);
        }
    }
}
