using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jarvis.common.dtos.Eventhandling;
using jarvis.common.dtos.Workflow;
using jarvis.server.entities.Workflow;
using jarvis.server.repositories;

namespace jarvis.server.model
{
    public interface IEventHandlingLogic
    {
        List<EventHandlerDto> GetAllEventHandler();
        void AddEntryInWorkflowQueue(WorkflowQueueDto workflowQueueDto);
    }

    public class EventHandlingLogic : IEventHandlingLogic
    {
        private readonly IEventHandlerRepository _eventHandlerRepository;
        private readonly IDefinedWorkflowRepository _definedWorkflowRepository;
        private readonly IWorkflowQueueRepository _workflowQueueRepository;

        public EventHandlingLogic(IEventHandlerRepository eventHandlerRepository, IDefinedWorkflowRepository definedWorkflowRepository, IWorkflowQueueRepository workflowQueueRepository)
        {
            _eventHandlerRepository = eventHandlerRepository;
            _definedWorkflowRepository = definedWorkflowRepository;
            _workflowQueueRepository = workflowQueueRepository;
        }

        public List<EventHandlerDto> GetAllEventHandler()
        {
            return _eventHandlerRepository.GetAllEventHandler().Select(e => new EventHandlerDto()
                                                                                {
                                                                                    Id = e.Id,
                                                                                    EventGroupTypes = e.EventGroupTypes,
                                                                                    EventType = e.EventType,
                                                                                    DefinedWorkflowId = e.DefinedWorkflow == null ? -1 : e.DefinedWorkflow.Id
                                                                                }).ToList();
        }

        public void AddEntryInWorkflowQueue(WorkflowQueueDto workflowQueueDto)
        {
            var workflow = _definedWorkflowRepository.GetWorkflow(new DefinedWorkflowFilterCriteria() {Id = workflowQueueDto.DefinedWorkflowId});


            var workflowQueue = _workflowQueueRepository.Create();
            workflowQueue.DefinedWorkflow = workflow;
            workflowQueue.QueueDate = DateTime.UtcNow;

            _workflowQueueRepository.Save(workflowQueue);

        }
    }

    
}
