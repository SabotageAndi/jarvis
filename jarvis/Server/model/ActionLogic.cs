using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using jarvis.client.common.ServiceClients;
using jarvis.common.domain;
using jarvis.common.dtos;
using jarvis.common.dtos.Actionhandling;
using jarvis.server.repositories;
using Action = jarvis.common.domain.Action;

namespace jarvis.server.model
{
    public interface IActionLogic
    {
        ActionResultDto Execute(ActionDto actionDto);
    }

    public class ActionLogic : IActionLogic
    {
        private readonly IClientRepository _clientRepository;

        public ActionLogic(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public ActionResultDto Execute(ActionDto actionDto)
        {
            //TODO: Factory/Registry/kA
            if (!(actionDto.ActionGroup == ActionGroup.File && actionDto.Action == Action.File_Delete))
            {
                return null;
            }

            var clientName = actionDto.Parameters.Where(p => p.Name == "Client").Single().Value;
            var filePath = actionDto.Parameters.Where(p => p.Name == "Path").Single().Value;
            var client = _clientRepository.GetByName(clientName);

            var actionDtoForClient = new ActionDto();
            actionDtoForClient.ActionGroup = ActionGroup.File;
            actionDtoForClient.Action = Action.File_Delete;
            actionDtoForClient.Parameters.Add(new ParameterDto(){Category = "File", Name = "Path", Value = filePath});


            var restClient = new JarvisRestClient();
            restClient.BaseUrl = client.Hostname;
            var request = restClient.CreateRequest("action/execute", Method.POST);
            request.AddBody(actionDtoForClient);

            return restClient.Execute<ActionResultDto>(request);
        }
    }
}
