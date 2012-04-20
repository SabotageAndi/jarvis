using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using jarvis.addins.actions;
using jarvis.addins.trigger;
using jarvis.client.common;
using jarvis.client.common.Actions;
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

            LoadTriggers();
            LoadActionHandlers();
       

            _actionServiceHost.Start();
        }

        private static IEnumerable<Type> GetAddinTypes(Assembly addin, Type addinType)
        {
            return addin.GetTypes().Where(type => !type.IsAbstract && type.IsSubclassOf(addinType));
        }

        private void LoadActionHandlers()
        {
            foreach (var addin in Addins)
            {
                var actionHandlerTypes = GetAddinTypes(addin, typeof(ActionHandler));
                foreach (var triggerType in actionHandlerTypes)
                {
                    var actionHandler = (ActionHandler)Activator.CreateInstance(triggerType);
                    _container.InjectProperties(actionHandler);

                    _actionRegistry.RegisterActionHandler(actionHandler);
                }
            }    
        }

        private void LoadTriggers()
        {
            foreach (var addin in Addins)
            {
                var triggerTypes = GetAddinTypes(addin, typeof (Trigger)); 
                foreach (var triggerType in triggerTypes)
                {
                    var trigger = (Trigger)Activator.CreateInstance(triggerType);
                    _container.InjectProperties(trigger);
                    Triggers.Add(trigger);
                }
            }

            foreach (var trigger in Triggers)
            {
                trigger.init();
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