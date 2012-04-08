using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using jarvis.common.dtos;

namespace jarvis.client.common.ServiceClients
{
    public interface IServerStatusService
    {
        bool isOnline();
    }

    public class ServerStatusService :  ServiceBase, IServerStatusService
    {
        public ServerStatusService(IJarvisRestClient jarvisRestClient, IConfiguration configuration) : base(jarvisRestClient, configuration)
        {
        }

        protected override string ServiceName
        {
            get { return _configuration.ServerStatusService; }
        }

        public bool isOnline()
        {
            var isOnlineRequest = JarvisRestClient.CreateRequest("server/status", Method.GET);

            return JarvisRestClient.Execute<ResultDto<bool>>(isOnlineRequest).Result;
        }
    }
}
