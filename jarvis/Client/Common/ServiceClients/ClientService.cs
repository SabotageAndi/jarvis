using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using jarvis.common.dtos.Management;

namespace jarvis.client.common.ServiceClients
{
    public interface IClientService
    {
        ClientDto Register(ClientDto clientDto);
        void Logon(ClientDto clientDto);
        void Logoff(ClientDto clientDto);
    }

    public class ClientService : ServiceBase, IClientService
    {
        public ClientService(IJarvisRestClient jarvisRestClient, IConfiguration configuration) : base(jarvisRestClient, configuration)
        {
        }

        protected override string ServiceName
        {
            get { return _configuration.ClientService; }
        }

        public ClientDto Register(ClientDto clientDto)
        {
            var registerRequest = JarvisRestClient.CreateRequest("client", Method.POST);
            registerRequest.AddBody(clientDto);

            return _jarvisRestClient.Execute<ClientDto>(registerRequest);
        }

        public void Logon(ClientDto clientDto)
        {
            var logonRequest = JarvisRestClient.CreateRequest("client/logon", Method.POST);
            logonRequest.AddBody(clientDto);

            _jarvisRestClient.Execute(logonRequest);
        }

        public void Logoff(ClientDto clientDto)
        {
            var logonRequest = JarvisRestClient.CreateRequest("client/logoff", Method.POST);
            logonRequest.AddBody(clientDto);

            _jarvisRestClient.Execute(logonRequest);
        }
    }
}
