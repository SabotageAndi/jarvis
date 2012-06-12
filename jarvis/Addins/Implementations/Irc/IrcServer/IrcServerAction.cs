using System.Linq;
using RestSharp;
using jarvis.addins.serverActions;
using jarvis.client.common.ServiceClients;
using jarvis.common.dtos.Actionhandling;

namespace jarvis.addins.ircserver
{
    public class IrcServerAction : ServerAction
    {
        public override string ActionGroup
        {
            get { return "Irc"; }
        }

        public override ActionResultDto Execute(ActionDto actionDto)
        {
            var clientName = actionDto.Parameters.Where(p => p.Name == "Client").Single().Value;
            var client = ClientRepository.GetByName(clientName);

            var restClient = new JarvisRestClient();
            restClient.BaseUrl = client.Hostname;
            var request = restClient.CreateRequest("action/execute", Method.POST);
            request.AddBody(actionDto);

            return restClient.Execute<ActionResultDto>(request);
        }

        public override bool CanExecute(ActionDto actionDto)
        {
            return actionDto.ActionGroup == "Irc";
        }
    }
}
