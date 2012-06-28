using System.Linq;
using RestSharp;
using jarvis.addins.serverActions;
using jarvis.client.common.ServiceClients;
using jarvis.common.dtos;
using jarvis.common.dtos.Actionhandling;
using jarvis.common.dtos.Requests;
using jarvis.server.common.Database;

namespace jarvis.addins.ircserver
{
    public class IrcServerAction : ServerAction
    {
        public override string ActionGroup
        {
            get { return "Irc"; }
        }

        protected override ActionResultDto ExecuteAction(ITransactionScope transactionScope, ActionDto actionDto)
        {
            var clientName = actionDto.Parameters.Where(p => p.Name == "Client").Single().Value;
            var client = ClientRepository.GetByName(transactionScope, clientName);

            var restClient = new JarvisRestClient(Log);
            restClient.BaseUrl = client.Hostname;

            return restClient.Execute<ResultDto<ActionResultDto>>(new ActionExecuteRequest(){ActionDto = actionDto}).Result;
        }

        protected override bool CanExecuteAction(ITransactionScope transactionScope, ActionDto actionDto)
        {
            return actionDto.ActionGroup == "Irc";
        }
    }
}
