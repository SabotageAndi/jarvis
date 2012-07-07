using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using jarvis.client.common;
using jarvis.client.common.ServiceClients;
using jarvis.common.dtos;
using jarvis.common.dtos.Eventhandling;
using jarvis.common.dtos.Requests;
using jarvis.common.dtos.Workflow;
using log4net;

namespace EventHandler
{
    class EventhandlerClient : Client
    {
        private readonly IEventHandlingService _eventHandlingService;
        private readonly JarvisRestClient _client;
        private DateTime _lastCheck;

        public EventhandlerClient(IClientService clientService,
            IConfiguration configuration,
            IServerStatusService serverStatusService,
            ILog log,
            IEventHandlingService eventHandlingService)
            : base(clientService, configuration, serverStatusService, log)
        {
            _eventHandlingService = eventHandlingService;
            _client = new JarvisRestClient(log);
            _client.BaseUrl = configuration.ServerUrl;
        }

        public override bool EnableAddins
        {
            get
            {
                return false;
            }
        }

        public override void Run()
        {
            base.Run();
            _lastCheck = DateTime.UtcNow;

            while (true)
            {
                Thread.Sleep(10000);
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
                else
                {
                    Log.Error(e);
                }
            }
        }

        private void ExecuteEventPipeline()
        {
            var eventHandlers = GetEventhandlers();
            var events = GetEvents();
            _lastCheck = DateTime.UtcNow;

            HandleEvents(events, eventHandlers);
        }

        private void HandleEvents(List<EventDto> events, List<EventHandlerDto> eventHandlers)
        {
            if (events == null)
            {
                return;
            }

            foreach (var eventDto in events)
            {
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

                    _eventHandlingService.AddWorkflowToQueue(workflowQueueDto);
                }
            }
        }

        private List<EventDto> GetEvents()
        {
            Log.InfoFormat("Getting Events since: {0:yyyy-MM-dd hh:mm:ss.nnn}", _lastCheck);
            return _eventHandlingService.GetEvents(_lastCheck);
        }

        private List<EventHandlerDto> GetEventhandlers()
        {
            return _eventHandlingService.GetEventhandlers();
        }
    }
}
