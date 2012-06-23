using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using jarvis.server.services;

namespace jarvis.server
{
    internal interface IServiceHostManager
    {
        void Start();
        void Stop();
    }

    class ServiceHostManager : IServiceHostManager
    {
        private readonly ServerStatusService _serverStatusService;
        private readonly ActionService _actionService;
        private readonly ClientService _clientService;
        private readonly EventHandlingService _eventHandlingService;
        private readonly TriggerService _triggerService;
        private readonly WorkflowService _workflowService;

        private readonly List<IService> _services;
        private readonly Dictionary<IService, ServiceHost> _serviceHosts; 

        public ServiceHostManager(ServerStatusService serverStatusService, ActionService actionService, ClientService clientService, 
            EventHandlingService eventHandlingService, TriggerService triggerService, WorkflowService workflowService)
        {
            _serverStatusService = serverStatusService;
            _actionService = actionService;
            _clientService = clientService;
            _eventHandlingService = eventHandlingService;
            _triggerService = triggerService;
            _workflowService = workflowService;

            _services = new List<IService>()
                            {
                                _serverStatusService,
                                _actionService,
                                _clientService,
                                _eventHandlingService,
                                _triggerService,
                                _workflowService
                            };
            _serviceHosts = new Dictionary<IService, ServiceHost>();
        }

        public void Start()
        {
            int port = 7777;


            foreach (var service in _services)
            {
                var baseAddress = GetBaseAddress(port);
                var serviceType = service.GetType();

                var contractDescription = ContractDescription.GetContract(serviceType);
          
                string serviceAddress = baseAddress + serviceType.Name;
                
                var endpointAddress = new EndpointAddress(serviceAddress);
                var binding = new WebHttpBinding();
                var host = new WebServiceHost(service, new Uri(serviceAddress));

                var serviceEndpoint = new ServiceEndpoint(contractDescription, binding, endpointAddress);
                var webHttpBehavior = new WebHttpBehavior();
                webHttpBehavior.FaultExceptionEnabled = true;
                webHttpBehavior.DefaultOutgoingRequestFormat = WebMessageFormat.Json;
                webHttpBehavior.DefaultOutgoingResponseFormat = WebMessageFormat.Json;
                

                serviceEndpoint.Behaviors.Add(webHttpBehavior);
                host.AddServiceEndpoint(serviceEndpoint);

                Console.WriteLine("Service " + serviceType.Name + ", Endpoint: " + endpointAddress.Uri);

                host.Open();
               
                _serviceHosts.Add(service, host);
                //port++;
            }
        }

        public void Stop()
        {
            foreach (var service in _services)
            {
                var serviceHost = _serviceHosts[service];
                serviceHost.Close();
            }
        }

        private String GetBaseAddress(int port)
        {
            return String.Format("http://{0}:{1}/", "localhost", port);
        }
    }
}
