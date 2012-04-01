using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jarvis.common.dtos.Workflow;
using jarvis.server.common.Database;
using jarvis.server.entities.Workflow;
using jarvis.server.repositories;

namespace jarvis.server.model
{
    public interface IWorkflowLogic
    {
        RunnedWorkflowDto GetWorkflowToExecute();
    }

    public class WorkflowLogic : IWorkflowLogic
    {
        private readonly IWorkflowQueueRepository _workflowQueueRepository;
        private readonly IRunnedWorkflowRepository _runnedWorkflowRepository;
        private readonly IRunnedWorkflowStepRepository _runnedWorkflowStepRepository;
        private readonly IRunnedTaskRepository _runnedTaskRepository;
        private readonly IRunnedNextWorkflowStepRepository _runnedNextWorkflowStepRepository;
        private readonly ITransactionProvider _transactionProvider;

        public WorkflowLogic(IWorkflowQueueRepository workflowQueueRepository, IRunnedWorkflowRepository runnedWorkflowRepository,
            IRunnedWorkflowStepRepository runnedWorkflowStepRepository, IRunnedTaskRepository runnedTaskRepository,
            IRunnedNextWorkflowStepRepository runnedNextWorkflowStepRepository, ITransactionProvider transactionProvider)
        {
            _workflowQueueRepository = workflowQueueRepository;
            _runnedWorkflowRepository = runnedWorkflowRepository;
            _runnedWorkflowStepRepository = runnedWorkflowStepRepository;
            _runnedTaskRepository = runnedTaskRepository;
            _runnedNextWorkflowStepRepository = runnedNextWorkflowStepRepository;
            _transactionProvider = transactionProvider;
        }

        public RunnedWorkflowDto GetWorkflowToExecute()
        {
            var workflowToExecute = _workflowQueueRepository.GetNextQueuedWorkflowAndSetStarttime();

            if (workflowToExecute == null)
                return null;

            var runnedWorkflow = CreateRunnedWorkflow(workflowToExecute.DefinedWorkflow);
            workflowToExecute.RunnedWorkflow = runnedWorkflow;

            _workflowQueueRepository.Save(workflowToExecute);

            _transactionProvider.CurrentScope.Flush();

            _runnedWorkflowRepository.Refresh(runnedWorkflow);

            return TranslateToDto(runnedWorkflow);
        }

        private RunnedWorkflowDto TranslateToDto(RunnedWorkflow runnedWorkflow)
        {
            var runnedWorkflowDto = new RunnedWorkflowDto
                                        {
                                            Id = runnedWorkflow.Id,
                                            WorkflowSteps = runnedWorkflow.WorkflowSteps.Select(TranslateToDto).ToList(),
                                            NextWorkflowSteps = runnedWorkflow.NextWorkflowSteps.Select(TranslateToDto).ToList(),
                                        };
            return runnedWorkflowDto;
        }

        private RunnedNextWorkflowStepDto TranslateToDto(RunnedNextWorkflowStep runnedNextWorkflowStep)
        {
            return new RunnedNextWorkflowStepDto()
                                                   {
                                                       Id = runnedNextWorkflowStep.Id,
                                                       NextStepId = runnedNextWorkflowStep.NextStep == null ? (int?)null : runnedNextWorkflowStep.NextStep.Id,
                                                       PreviousStepId = runnedNextWorkflowStep.PreviousStep == null ? (int?)null : runnedNextWorkflowStep.PreviousStep.Id
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

        private RunnedWorkflow CreateRunnedWorkflow(DefinedWorkflow definedWorkflow)
        {
            var runnedWorkflow = _runnedWorkflowRepository.Create();
            runnedWorkflow.DefinedWorkflow = definedWorkflow;
            _runnedWorkflowRepository.Save(runnedWorkflow);

            var runnedWorkflowSteps = CreateRunnedWorkflowSteps(definedWorkflow.WorkflowSteps, runnedWorkflow);
            CreateRunnedNextWorkflowSteps(definedWorkflow.NextWorkflowSteps, runnedWorkflow, runnedWorkflowSteps);

            return runnedWorkflow;
        }

        private void CreateRunnedNextWorkflowSteps(IList<DefinedNextWorkflowStep> nextWorkflowSteps, RunnedWorkflow runnedWorkflow, List<RunnedWorkflowStep> runnedWorkflowSteps)
        {
            foreach (var definedNextWorkflowStep in nextWorkflowSteps)
            {
                var runnedNextWorkflowStep = _runnedNextWorkflowStepRepository.Create();
                runnedNextWorkflowStep.RunnedWorkflow = runnedWorkflow;
                runnedNextWorkflowStep.DefinedNextWorkflowStep = definedNextWorkflowStep;

                if (definedNextWorkflowStep.NextStep == null)
                    runnedNextWorkflowStep.NextStep = null;
                else
                {
                    runnedNextWorkflowStep.NextStep = runnedWorkflowSteps.Where(rws => rws.DefinedWorkflowStep == definedNextWorkflowStep.NextStep).Single();
                }

                if (definedNextWorkflowStep.PreviousStep == null)
                    runnedNextWorkflowStep.PreviousStep = null;
                else
                {
                    runnedNextWorkflowStep.PreviousStep = runnedWorkflowSteps.Where(rws => rws.DefinedWorkflowStep == definedNextWorkflowStep.PreviousStep).Single();
                }


                _runnedNextWorkflowStepRepository.Save(runnedNextWorkflowStep);
            }
        }

        private List<RunnedWorkflowStep> CreateRunnedWorkflowSteps(IList<DefinedWorkflowStep> workflowSteps, RunnedWorkflow runnedWorkflow)
        {
            var runnedWorkflowSteps = new List<RunnedWorkflowStep>();

            foreach (var definedWorkflowStep in workflowSteps)
            {
                var runnedWorkflowStep = _runnedWorkflowStepRepository.Create();
                runnedWorkflowStep.RunnedWorkflow = runnedWorkflow;
                runnedWorkflowStep.DefinedWorkflowStep = definedWorkflowStep;
                runnedWorkflowStep.RunnedTask = CreateRunnedTask(definedWorkflowStep.DefinedTask);


                _runnedWorkflowStepRepository.Save(runnedWorkflowStep);

                runnedWorkflowSteps.Add(runnedWorkflowStep);
            }

            return runnedWorkflowSteps;
        }

        private RunnedTask CreateRunnedTask(DefinedTask definedTask)
        {
            var runnedTask = _runnedTaskRepository.Create();
            runnedTask.DefinedTask = definedTask;
            runnedTask.RunCode = definedTask.RunCode;

            _runnedTaskRepository.Save(runnedTask);

            return runnedTask;
        }
    }
}
