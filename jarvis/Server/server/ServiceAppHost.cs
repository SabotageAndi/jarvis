using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Funq;
using NHibernate;
using Ninject;
using ServiceStack.Logging;
using ServiceStack.Logging.Support.Logging;
using ServiceStack.ServiceInterface;
using ServiceStack.WebHost.Endpoints;
using jarvis.common.dtos.Requests;
using jarvis.common.logic;
using jarvis.server.services;
using jarvis.server.services.client;
using jarvis.server.services.eventhandling;
using jarvis.server.services.workflow;

namespace jarvis.server
{
    class ServiceAppHost : AppHostHttpListenerBase
    {
        private readonly IKernel _kernel;

        public ServiceAppHost(IKernel kernel)
            : base("Jarvis", Assembly.GetExecutingAssembly())
        {

            _kernel = kernel;
        }

        public override void Configure(Container container)
        {
            Plugins.Add(new SessionFeature());


            container.Adapter = new NinjectIocAdapter(_kernel);

            base.SetConfig(new EndpointHostConfig
            {
                GlobalResponseHeaders =
                {
                    { "Access-Control-Allow-Origin", "*" },
                    { "Access-Control-Allow-Methods", "GET, POST, PUT, DELETE, OPTIONS" },
                },

                WsdlServiceNamespace = "http://www.servicestack.net/types",
                DebugMode = true
            });

            Routes.Add<ServiceStatusRequest>("/serverstatus", "GET");
            Routes.Add<RegisterClientRequest>("/client/register", "POST");
            Routes.Add<LoginClientRequest>("/client/login", "PUT");
            Routes.Add<LogoffClientRequest>("/client/logoff", "PUT");
            Routes.Add<ActionExecuteRequest>("/action/execute", "POST");
            Routes.Add<TriggerRequest>("/trigger", "POST");
            Routes.Add<GetAllEventsSinceRequest>("/events/{ticks}", "POST");
            Routes.Add<GetEventHandlerRequest>("/eventhandler", "GET");
            Routes.Add<AddWorkflowInQueueRequest>("/workflowqueue", "POST");
            Routes.Add<GetWorkflowToExecuteRequest>("/workflow/gettoexecute", "POST");


            LogManager.LogFactory = new ConsoleLogFactory();
        }
    }
}
