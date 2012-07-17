using System;
using System.Threading;

namespace jarvis.client.worker
{
    public interface IWorkerLockManager
    {
        void Block();
        void Release();
    }

    class WorkerLockManager : IWorkerLockManager
    {
        private Semaphore _semaphore;

        public WorkerLockManager()
        {
            _semaphore = new Semaphore(0,Int32.MaxValue);
        }

        public void Block()
        {
            _semaphore.WaitOne();
        }

        public void Release()
        {
            _semaphore.Release();
        }
    }
}
