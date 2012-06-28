using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Funq;
using Ninject;
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

            Routes.Add<ServiceStatusRequest>("/serverstatus");
            Routes.Add<RegisterClientRequest>("/client/register");
            Routes.Add<LoginClientRequest>("/client/login");
            Routes.Add<LogoffClientRequest>("/client/logoff");
            Routes.Add<ActionExecuteRequest>("/action/execute");
            Routes.Add<TriggerRequest>("/trigger");
            Routes.Add<GetAllEventsSinceRequest>("/events/{ticks}");
            Routes.Add<GetEventHandlerRequest>("/eventhandler");
            Routes.Add<AddWorkflowInQueueRequest>("/workflowqueue");
            Routes.Add<GetWorkflowToExecuteRequest>("/workflow/gettoexecute");
        }
    }
}
