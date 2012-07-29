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
        private bool _isRunning;

        public WorkerLockManager()
        {
            _semaphore = new Semaphore(0,Int32.MaxValue);
        }

        public void Block()
        {
            _isRunning = true;
            _semaphore.WaitOne();
        }

        public void Release()
        {
            if (!_isRunning)
                return;

            _semaphore.Release();
            _isRunning = false;
        }
    }
}
