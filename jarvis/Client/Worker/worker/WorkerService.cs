using ServiceStack.ServiceInterface;
using jarvis.client.common.ServiceClients;
using jarvis.common.dtos;
using jarvis.common.dtos.Requests;
using log4net;

namespace jarvis.client.worker
{
    class WorkerService : ServiceBase<WorkerTriggerRequest>
    {
        private readonly IWorkerLockManager _workerLockManager;
        private readonly ILog _log;

        public WorkerService(IWorkerLockManager workerLockManager, ILog log)
        {
            _workerLockManager = workerLockManager;
            _log = log;
        }

        protected override object HandleException(WorkerTriggerRequest request, System.Exception ex)
        {            _log.FatalFormat("Error at executing action: {0}", ex);
            return base.HandleException(request, ex);
        }

        protected override object Run(WorkerTriggerRequest request)
        {
            _workerLockManager.Release();
            _log.Info("Worker triggered");

            return new ResultDto();
        }


    }
}
