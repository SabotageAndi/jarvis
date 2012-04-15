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
        private readonly ActionService _actionService;
        private ServiceHost _serviceHost;
        private string _addressRoot = "http://localhost:8888/";

        public ActionServiceHost(ActionService actionService )
        {
            _actionService = actionService;
        }

        public void Start()
        {
            _serviceHost =  new ServiceHost(_actionService, new Uri(_addressRoot));
            _serviceHost.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.None;
            

            WebHttpBinding restBinding = new WebHttpBinding(WebHttpSecurityMode.None);
            
            
            var restEndPoint = new ServiceEndpoint(ContractDescription.GetContract(typeof (ActionService)), 
                restBinding, 
                new EndpointAddress(_addressRoot + "action"));


            _serviceHost.AddServiceEndpoint(restEndPoint);
            _serviceHost.Faulted += _serviceHost_Faulted;
            _serviceHost.UnknownMessageReceived += _serviceHost_UnknownMessageReceived;
            _serviceHost.Open();
        }

        void _serviceHost_UnknownMessageReceived(object sender, UnknownMessageReceivedEventArgs e)
        {
        }

        void _serviceHost_Faulted(object sender, EventArgs e)
        {

        }
    }
}
