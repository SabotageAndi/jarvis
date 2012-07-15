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

        public EventhandlingLockManager()
        {
            _semaphore = new Semaphore(0,1);
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
