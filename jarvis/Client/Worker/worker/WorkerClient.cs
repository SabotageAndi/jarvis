using System;
using System.Collections.Generic;
using System.Threading;
using Ninject;
using jarvis.addins.actions;
using jarvis.client.common;
using jarvis.client.common.ServiceClients;
using log4net;

namespace jarvis.client.worker
{
    public class WorkerClient : Client
    {
        private readonly IWorkflowEngine _workflowEngine;

        public WorkerClient(IClientService clientService, IConfiguration configuration, IServerStatusService serverStatusService,
                            IWorkflowEngine workflowEngine, ILog log)
            : base(clientService, configuration, serverStatusService, log)
        {
            _workflowEngine = workflowEngine;
        }


        public override void Init(IKernel container)
        {
            base.Init(container);

            LoadActions();
        }

        private void LoadActions()
        {
            foreach (var addin in Addins)
            {
                var actionTypes = GetAddinTypes(addin, typeof (ClientAction));
                foreach (var actionType in actionTypes)
                {
                    Container.Bind(actionType).ToSelf().InSingletonScope();
                }

                foreach (var actionType in actionTypes)
                {
                    var action = (ClientAction)Container.Get(actionType);
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