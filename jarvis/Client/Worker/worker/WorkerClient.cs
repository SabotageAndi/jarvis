using System;
using System.Collections.Generic;
using System.Threading;
using Autofac;
using jarvis.client.common;
using jarvis.client.common.ServiceClients;
using Action = jarvis.addins.actions.Action;

namespace jarvis.client.worker
{
    public class WorkerClient : Client
    {
        private readonly IWorkflowEngine _workflowEngine;

        public WorkerClient(IClientService clientService, IConfiguration configuration, IServerStatusService serverStatusService,
                            IWorkflowEngine workflowEngine)
            : base(clientService, configuration, serverStatusService)
        {
            _workflowEngine = workflowEngine;
        }


        public override void Init(IContainer container)
        {
            base.Init(container);

            LoadActions();
        }

        private void LoadActions()
        {
            foreach (var addin in Addins)
            {
                var updateContainer = new ContainerBuilder();
                var actionTypes = GetAddinTypes(addin, typeof (Action));
                foreach (var actionType in actionTypes)
                {
                    updateContainer.RegisterType(actionType).SingleInstance();
                }
                updateContainer.Update(_container);

                foreach (var actionType in actionTypes)
                {
                    var action = (Action) _container.Resolve(actionType);
                    _workflowEngine.AddAction(action);
                }
            }
        }

        public override void Run()
        {
            base.Run();
            while (true)
            {
                if (!_workflowEngine.Do())
                {
                    Thread.Sleep(10000);
                }
            }
        }
    }
}