using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using jarvis.addins.trigger;
using jarvis.client.common;
using jarvis.client.common.Actions;
using jarvis.client.common.Actions.ActionHandlers;
using jarvis.client.common.ServiceClients;

namespace jarvis.client
{
    public class ActionTriggerClient : Client
    {
        private readonly IActionServiceHost _actionServiceHost;
        private readonly IActionRegistry _actionRegistry;

        public ActionTriggerClient(IClientService clientService, IConfiguration configuration, IServerStatusService serverStatusService,
                                   IActionServiceHost actionServiceHost, IActionRegistry actionRegistry)
            : base(clientService, configuration, serverStatusService)
        {
            _actionServiceHost = actionServiceHost;
            _actionRegistry = actionRegistry;
            Triggers = new List<Trigger>();
        }

        private List<Trigger> Triggers { get; set; }

        public override void Init(IContainer container)
        {
            base.Init(container);

            _actionRegistry.RegisterActionHandler(new FileActionHandler());

            _actionServiceHost.Start();

            LoadTriggers();

            foreach (var trigger in Triggers)
            {
                trigger.init();
            }
        }

        private void LoadTriggers()
        {
            foreach (var addin in Addins)
            {
                var triggerTypes = addin.GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(typeof (Trigger)));
                foreach (var triggerType in triggerTypes)
                {
                    var trigger = (Trigger)Activator.CreateInstance(triggerType);
                    _container.InjectProperties(trigger);
                    Triggers.Add(trigger);
                }
            }
        }

        public override void Shutdown()
        {
            foreach (var trigger in Triggers)
            {
                trigger.deinit();
            }

            base.Shutdown();
        }

        public override void Run()
        {
            foreach (var trigger in Triggers)
            {
                trigger.run();
            }

            base.Run();
        }
    }
}