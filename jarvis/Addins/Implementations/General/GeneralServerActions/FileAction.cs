using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RestSharp;
using jarvis.addins.serverActions;
using jarvis.client.common.ServiceClients;
using jarvis.common.dtos;
using jarvis.common.dtos.Actionhandling;
using jarvis.common.dtos.Requests;
using jarvis.server.common.Database;
using jarvis.server.repositories;

namespace jarvis.addins.generalserveractions
{
    public class FileAction : ServerAction
    {
        protected override bool CanExecuteAction(ITransactionScope transactionScope, ActionDto actionDto)
        {
            return actionDto.ActionGroup == "File";
        }

        public override string ActionGroup
        {
            get { return "File"; }
        }

        protected override ActionResultDto ExecuteAction(ITransactionScope transactionScope, ActionDto actionDto)
        {
            if (actionDto.Action == "Move" || actionDto.Action == "Copy")
            {
                var sourceClientParameter = GetParameter(actionDto, "File", "SourceClient");
                var targetClientParameter = GetParameter(actionDto, "File", "TargetClient");

                if (sourceClientParameter.Value == targetClientParameter.Value)
                {
                    return ExecuteFileAction(transactionScope, actionDto);
                }

                var sourcePathParameter = GetParameter(actionDto, "File", "SourcePath");
                var targetPathParameter = GetParameter(actionDto, "File", "TargetPath");

                var readDataAction = new ActionDto()
                                         {
                                             ActionGroup = "File",
                                             Action = "Read",
                                             Parameters = new List<ParameterDto>()
                                                              {
                                                                  new ParameterDto() { Category = "File", Name = "Path", Value = sourcePathParameter.Value }
                                                              }
                                         };

                var readDataResult = ExecuteFileAction(transactionScope, readDataAction);

                var writeDataAction = new ActionDto()
                                        {
                                            ActionGroup = "File",
                                            Action = "Write",
                                            Parameters = new List<ParameterDto>()
                                                                                      {
                                                                                          new ParameterDto() { Category = "File", Name = "Path", Value = targetPathParameter.Value },
                                                                                          new ParameterDto() {Category = "File", Name = "Data", Value = readDataResult.Data}
                                                                                      }
                                        };

                ExecuteFileAction(transactionScope, writeDataAction);

                return new ActionResultDto();
            }
            
            return ExecuteFileAction(transactionScope, actionDto);
        }

        private ActionResultDto ExecuteFileAction(ITransactionScope transactionScope, ActionDto actionDto)
        {
            var clientName = actionDto.Parameters.Where(p => p.Name == "Client").Single().Value;
            var client = ClientRepository.GetClientsByFilterCriteria(transactionScope, new ClientFilterCriteria() {Name = clientName}).SingleOrDefault();

            var restClient = new JarvisRestClient(this.Log);
            restClient.BaseUrl = client.Hostname;

            return restClient.Execute<ResultDto<ActionResultDto>>(new ActionExecuteRequest(){ActionDto = actionDto}, "POST").Result;
        }
    }
}
