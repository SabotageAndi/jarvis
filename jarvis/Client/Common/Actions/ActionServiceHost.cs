using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace jarvis.client.common.Actions
{
    public interface IActionServiceHost
    {
        void Start();
    }

    public class ActionServiceHost : IActionServiceHost
    {
        private readonly IActionService _actionService;
        private readonly IConfiguration _configuration;
        private ServiceHost _serviceHost;

        public ActionServiceHost(IActionService actionService, IConfiguration configuration)
        {
            _actionService = actionService;
            _configuration = configuration;
        }

        public void Start()
        {
            var baseAddress = GetBaseAddress();
           
            var contractDescription = ContractDescription.GetContract(typeof (ActionService));
            var restBinding = new WebHttpBinding(WebHttpSecurityMode.None);
            var endpointAddress = new EndpointAddress(baseAddress + "action");

            var restEndPoint = new ServiceEndpoint(contractDescription, restBinding, endpointAddress);

            restEndPoint.Behaviors.Add(new WebHttpBehavior());

            _serviceHost =  new ServiceHost(_actionService, new Uri(baseAddress));
            _serviceHost.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.None;
            _serviceHost.AddServiceEndpoint(restEndPoint);
            _serviceHost.Faulted += _serviceHost_Faulted;
            _serviceHost.UnknownMessageReceived += _serviceHost_UnknownMessageReceived;
            _serviceHost.Open();
        }

        private String GetBaseAddress()
        {
            return String.Format("http://{0}:{1}/", "localhost", _configuration.LocalPort);
        }

        void _serviceHost_UnknownMessageReceived(object sender, UnknownMessageReceivedEventArgs e)
        {
        }

        void _serviceHost_Faulted(object sender, EventArgs e)
        {

        }
    }
}
