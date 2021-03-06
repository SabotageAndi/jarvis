﻿// J.A.R.V.I.S. - Just A Rather Very Intelligent System
// Copyright (C) 2012 Andreas Willich <sabotageandi@gmail.com>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Collections.Generic;
using System.Linq;
using jarvis.common.domain;
using jarvis.common.dtos;
using jarvis.common.dtos.Workflow;
using jarvis.server.common.Database;
using jarvis.server.entities.Eventhandling;
using jarvis.server.entities.Workflow;
using jarvis.server.repositories;

namespace jarvis.server.model
{
    public interface IWorkflowLogic
    {
        RunnedWorkflowDto GetWorkflowToExecute(ITransactionScope transactionScope);
    }

    public class WorkflowLogic : IWorkflowLogic
    {
        private readonly IParameterRepository _parameterRepository;
        private readonly IRunnedNextWorkflowStepRepository _runnedNextWorkflowStepRepository;
        private readonly IRunnedTaskRepository _runnedTaskRepository;
        private readonly IRunnedWorkflowRepository _runnedWorkflowRepository;
        private readonly IRunnedWorkflowStepRepository _runnedWorkflowStepRepository;
        private readonly IWorkflowQueueRepository _workflowQueueRepository;

        public WorkflowLogic(IWorkflowQueueRepository workflowQueueRepository, IRunnedWorkflowRepository runnedWorkflowRepository,
                             IRunnedWorkflowStepRepository runnedWorkflowStepRepository, IRunnedTaskRepository runnedTaskRepository,
                             IRunnedNextWorkflowStepRepository runnedNextWorkflowStepRepository, IParameterRepository parameterRepository)
        {
            _workflowQueueRepository = workflowQueueRepository;
            _runnedWorkflowRepository = runnedWorkflowRepository;
            _runnedWorkflowStepRepository = runnedWorkflowStepRepository;
            _runnedTaskRepository = runnedTaskRepository;
            _runnedNextWorkflowStepRepository = runnedNextWorkflowStepRepository;
            _parameterRepository = parameterRepository;
        }

        public RunnedWorkflowDto GetWorkflowToExecute(ITransactionScope transactionScope)
        {
            var workflowToExecute = _workflowQueueRepository.GetNextQueuedWorkflowAndSetStarttime(transactionScope);

            if (workflowToExecute == null)
            {
                return null;
            }

            var runnedWorkflow = CreateRunnedWorkflow(transactionScope, workflowToExecute.DefinedWorkflow, workflowToExecute.Event);
            workflowToExecute.RunnedWorkflow = runnedWorkflow;

            _workflowQueueRepository.Save(transactionScope, workflowToExecute);

            transactionScope.Flush();

            _runnedWorkflowRepository.Refresh(transactionScope, runnedWorkflow);

            return TranslateToDto(runnedWorkflow);
        }

        private RunnedWorkflowDto TranslateToDto(RunnedWorkflow runnedWorkflow)
        {
            var runnedWorkflowDto = new RunnedWorkflowDto
                                        {
                                            Id = runnedWorkflow.Id,
                                            WorkflowSteps = runnedWorkflow.WorkflowSteps.Select(TranslateToDto).ToList(),
                                            NextWorkflowSteps = runnedWorkflow.NextWorkflowSteps.Select(TranslateToDto).ToList(),
                                            Parameters = runnedWorkflow.Parameters.Select(TranslateToDto).ToList(),
                                        };
            return runnedWorkflowDto;
        }

        private ParameterDto TranslateToDto(Parameter parameter)
        {
            return new ParameterDto()
                       {
                           Id = parameter.Id,
                           Category = parameter.Category,
                           Name = parameter.Name,
                           Value = parameter.Value,
                       };
        }

        private RunnedNextWorkflowStepDto TranslateToDto(RunnedNextWorkflowStep runnedNextWorkflowStep)
        {
            return new RunnedNextWorkflowStepDto()
                       {
                           Id = runnedNextWorkflowStep.Id,
                           NextStepId = runnedNextWorkflowStep.NextStep == null ? (int?) null : runnedNextWorkflowStep.NextStep.Id,
                           PreviousStepId =
                               runnedNextWorkflowStep.PreviousStep == null ? (int?) null : runnedNextWorkflowStep.PreviousStep.Id
                       };
        }

        private RunnedWorkflowStepDto TranslateToDto(RunnedWorkflowStep runnedWorkflowStep)
        {
            return new RunnedWorkflowStepDto
                       {
                           Id = runnedWorkflowStep.Id,
                           RunnedTask = TranslateToDto(runnedWorkflowStep.RunnedTask)
                       };
        }

        private RunnedTaskDto TranslateToDto(RunnedTask runnedTask)
        {
            return new RunnedTaskDto()
                       {
                           Id = runnedTask.Id,
                           Name = runnedTask.DefinedTask.Name,
                           RunCode = runnedTask.RunCode,
                       };
        }

        private RunnedWorkflow CreateRunnedWorkflow(ITransactionScope transactionScope, DefinedWorkflow definedWorkflow, Event eventInformation)
        {
            var runnedWorkflow = _runnedWorkflowRepository.Create();
            runnedWorkflow.DefinedWorkflow = definedWorkflow;
            _runnedWorkflowRepository.Save(transactionScope, runnedWorkflow);

            var runnedWorkflowSteps = CreateRunnedWorkflowSteps(transactionScope, definedWorkflow.WorkflowSteps, runnedWorkflow);
            CreateRunnedNextWorkflowSteps(transactionScope, definedWorkflow.NextWorkflowSteps, runnedWorkflow, runnedWorkflowSteps);

            CreateParameters(transactionScope, runnedWorkflow, eventInformation);

            return runnedWorkflow;
        }

        private void CreateParameters(ITransactionScope transactionScope, RunnedWorkflow runnedWorkflow, Event eventInformation)
        {
            var reader = JsonParser.GetJsonSerializer().Deserialize<Dictionary<string, string>>(eventInformation.Data);

            _parameterRepository.Save(transactionScope, _parameterRepository.Create(runnedWorkflow, "EventParameter", "Client", eventInformation.Client.Name));
            _parameterRepository.Save(transactionScope, _parameterRepository.Create(runnedWorkflow, "EventParameter", "EventGroupType", eventInformation.EventGroupType));
            _parameterRepository.Save(transactionScope, _parameterRepository.Create(runnedWorkflow, "EventParameter", "EventType", eventInformation.EventType));

            foreach (var row in reader)
            {
                var parameter = _parameterRepository.Create(runnedWorkflow, "EventParameter", row.Key, row.Value);

                _parameterRepository.Save(transactionScope, parameter);
            }
        }

        private void CreateRunnedNextWorkflowSteps(ITransactionScope transactionScope,
                                                   IList<DefinedNextWorkflowStep> nextWorkflowSteps, 
                                                   RunnedWorkflow runnedWorkflow,
                                                   List<RunnedWorkflowStep> runnedWorkflowSteps)
        {
            foreach (var definedNextWorkflowStep in nextWorkflowSteps)
            {
                var runnedNextWorkflowStep = _runnedNextWorkflowStepRepository.Create();
                runnedNextWorkflowStep.RunnedWorkflow = runnedWorkflow;
                runnedNextWorkflowStep.DefinedNextWorkflowStep = definedNextWorkflowStep;

                if (definedNextWorkflowStep.NextStep == null)
                {
                    runnedNextWorkflowStep.NextStep = null;
                }
                else
                {
                    runnedNextWorkflowStep.NextStep =
                        runnedWorkflowSteps.Where(rws => rws.DefinedWorkflowStep == definedNextWorkflowStep.NextStep).Single();
                }

                if (definedNextWorkflowStep.PreviousStep == null)
                {
                    runnedNextWorkflowStep.PreviousStep = null;
                }
                else
                {
                    runnedNextWorkflowStep.PreviousStep =
                        runnedWorkflowSteps.Where(rws => rws.DefinedWorkflowStep == definedNextWorkflowStep.PreviousStep).Single();
                }


                _runnedNextWorkflowStepRepository.Save(transactionScope, runnedNextWorkflowStep);
            }
        }

        private List<RunnedWorkflowStep> CreateRunnedWorkflowSteps(ITransactionScope transactionScope, IList<DefinedWorkflowStep> workflowSteps, RunnedWorkflow runnedWorkflow)
        {
            var runnedWorkflowSteps = new List<RunnedWorkflowStep>();

            foreach (var definedWorkflowStep in workflowSteps)
            {
                var runnedWorkflowStep = _runnedWorkflowStepRepository.Create();
                runnedWorkflowStep.RunnedWorkflow = runnedWorkflow;
                runnedWorkflowStep.DefinedWorkflowStep = definedWorkflowStep;
                runnedWorkflowStep.RunnedTask = CreateRunnedTask(transactionScope, definedWorkflowStep.DefinedTask);


                _runnedWorkflowStepRepository.Save(transactionScope, runnedWorkflowStep);

                runnedWorkflowSteps.Add(runnedWorkflowStep);
            }

            return runnedWorkflowSteps;
        }

        private RunnedTask CreateRunnedTask(ITransactionScope transactionScope, DefinedTask definedTask)
        {
            var runnedTask = _runnedTaskRepository.Create();
            runnedTask.DefinedTask = definedTask;
            runnedTask.RunCode = definedTask.RunCode;

            _runnedTaskRepository.Save(transactionScope, runnedTask);

            return runnedTask;
        }
    }
}