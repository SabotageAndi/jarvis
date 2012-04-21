using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using jarvis.addins.serverActions;
using jarvis.client.common.ServiceClients;
using jarvis.common.dtos.Actionhandling;

namespace jarvis.addins.generalserveractions
{
    public class FileAction : ServerAction 
    {
        public override bool CanExecute(ActionDto actionDto)
        {
            if (!(actionDto.ActionGroup == "File" && actionDto.Action == "Delete"))
            {
                return false;
            }

            return true;

        }

        public override string ActionGroup
        {
            get { return "File"; }
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
    }
}
