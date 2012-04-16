using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using jarvis.common.dtos.Actionhandling;

namespace jarvis.client.common.ServiceClients
{
    public interface IActionService
    {
        ActionResultDto Execute(ActionDto actionDto);
    }

    public class ActionService : ServiceBase, IActionService
    {
        public ActionService(IJarvisRestClient jarvisRestClient, IConfiguration configuration) : base(jarvisRestClient, configuration)
        {
        }

        protected override string ServiceName
        {
            get { return "ActionService"; }
        }

        public ActionResultDto Execute(ActionDto actionDto)
        {
            var request = JarvisRestClient.CreateRequest("action", Method.POST);
            request.AddBody(actionDto);

            return JarvisRestClient.Execute<ActionResultDto>(request);
        }
    }
}
