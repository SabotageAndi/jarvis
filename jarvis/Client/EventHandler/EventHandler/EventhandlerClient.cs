﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using jarvis.client.common;
using jarvis.client.common.ServiceClients;
using jarvis.common.domain;
using jarvis.common.dtos.Eventhandling;
using jarvis.common.dtos.Workflow;
using log4net;

namespace jarvis.client.eventhandler
{
    class EventhandlerClient : Client
    {
        private readonly IEventhandlingServiceHost _eventhandlingServiceHost;
        private IEventHandlingService _eventHandlingService;
        private readonly IEventhandlingLockManager _eventhandlingLockManager;
        private DateTime _lastCheck;

        public EventhandlerClient(IClientService clientService,
            IConfiguration configuration,
            IServerStatusService serverStatusService,
            ILog log,
            IEventhandlingServiceHost eventhandlingServiceHost, IEventHandlingService eventHandlingService,
            IEventhandlingLockManager eventhandlingLockManager)
            : base(clientService, configuration, serverStatusService, log)
        {
            _eventhandlingServiceHost = eventhandlingServiceHost;
            _eventHandlingService = eventHandlingService;
            _eventhandlingLockManager = eventhandlingLockManager;
        }

        public override bool EnableAddins
        {
            get
            {
                return false;
            }
        }

        protected override ClientTypeEnum Type
        {
            get { return ClientTypeEnum.Eventhandler; }
        }

        public override void Init(Ninject.IKernel container)
        {
            _lastCheck = DateTime.UtcNow;

            base.Init(container);

            _eventhandlingServiceHost.Init();
        }

        public override void Run()
        {
            base.Run();
            _eventhandlingServiceHost.Start();


            while (true)
            {
                Log.Info("Waiting for event");
                _eventhandlingLockManager.Block();
                Log.Info("Event triggered");
                Do();
            }
        }

        private void Do()
        {
            try
            {
                ExecuteEventPipeline();
            }
            catch (Exception e)
            {
                var webEx = e as WebException;
                if (webEx != null)
                {
                    if (webEx.Status == WebExceptionStatus.ReceiveFailure)
                    {
                        try
                        {
                            ExecuteEventPipeline();
                        }
                        catch (Exception exception)
                        {
                            Log.Error(exception);
                        }
                    }
                    Log.ErrorFormat("Response {0}\n, Status: {1}\n, Data: {2}", webEx.Response, webEx.Status, webEx.Data);
                }
                
                Log.Error(e);
            }
        }

        private void ExecuteEventPipeline()
        {
            var eventHandlers = GetEventhandlers();

            if (Log.IsDebugEnabled)
            {
                var eventHandlersString = String.Join(",", eventHandlers.Select(e => e.Id.ToString()).ToArray());
                Log.Debug("Eventhandlers: " + eventHandlersString);
            }

            var eventToProcess = GetEventToProcess();

            while (eventToProcess != null && eventToProcess.Id != -1)
            {
                HandleEvent(eventHandlers, eventToProcess);
                eventToProcess = GetEventToProcess();
            }

        }

        private void HandleEvent(List<EventHandlerDto> eventHandlers, EventDto eventDto)
        {
            Log.InfoFormat("EventGroupType: {0}\nEventType: {1}\nClientId: {2}\nTriggeredDate: {3}", eventDto.EventGroupType, eventDto.EventType, eventDto.ClientId, eventDto.TriggeredDate);


            var hittedEventHandler = from eh in eventHandlers
                                     where (eh.EventGroupType == null || eh.EventGroupType == eventDto.EventGroupType)
                                           && (eh.EventType == null || eh.EventType == eventDto.EventType)
                                     select eh;

            foreach (var eventHandlerDto in hittedEventHandler)
            {
                var workflowQueueDto = new WorkflowQueueDto
                    {
                        EventHandlerId = eventHandlerDto.Id,
                        DefinedWorkflowId = eventHandlerDto.DefinedWorkflowId,
                        EventId = eventDto.Id
                    };

                Log.InfoFormat("Hitted Eventhandler: {0}", eventHandlerDto.Id);

                _eventHandlingService.AddWorkflowToQueue(workflowQueueDto);
            }
        }

        private EventDto GetEventToProcess()
        {
            return _eventHandlingService.GetEventToProcess();
        }

        private List<EventHandlerDto> GetEventhandlers()
        {
            return _eventHandlingService.GetEventhandlers();
        }

    }
}
