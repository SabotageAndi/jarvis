using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace jarvis.client.eventhandler
{
    internal interface IEventhandlingLockManager
    {
        void Block();
        void Release();
    }

    class EventhandlingLockManager : IEventhandlingLockManager
    {
        private Semaphore _semaphore;
        private bool _isRunning;

        public EventhandlingLockManager()
        {
            _semaphore = new Semaphore(0,1);
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
