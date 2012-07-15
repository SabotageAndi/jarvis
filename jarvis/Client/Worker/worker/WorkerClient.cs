using System;
using System.Collections.Generic;
using System.Threading;
using Ninject;
using jarvis.addins.actions;
using jarvis.client.common;
using jarvis.client.common.ServiceClients;
using jarvis.common.domain;
using log4net;

namespace jarvis.client.worker
{
    public class WorkerClient : Client
    {
        private readonly IWorkflowEngine _workflowEngine;
        private readonly IWorkerLockManager _workerLockManager;
        private readonly IWorkerServiceHost _workerServiceHost;

        public WorkerClient(IClientService clientService, IConfiguration configuration, IServerStatusService serverStatusService,
                            IWorkflowEngine workflowEngine, ILog log, IWorkerLockManager workerLockManager, IWorkerServiceHost workerServiceHost)
            : base(clientService, configuration, serverStatusService, log)
        {
            _workflowEngine = workflowEngine;
            _workerLockManager = workerLockManager;
            _workerServiceHost = workerServiceHost;
        }


        protected override ClientTypeEnum Type
        {
            get { return ClientTypeEnum.Worker; }
        }

        public override void Init(IKernel container)
        {
            base.Init(container);

            _workerServiceHost.Init();

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

            _workerServiceHost.Start();

            while (true)
            {
                _workerLockManager.Block();

                while (_workflowEngine.Do())
                {
                }
            }
        }
    }
}