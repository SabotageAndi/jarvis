using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq;
using jarvis.server.common.Database;
using jarvis.server.entities.Eventhandling;
using EventHandler = jarvis.server.entities.Eventhandling.EventHandler;

namespace jarvis.server.repositories
{
    public interface IEventHandlerRepository
    {
        IQueryable<EventHandler> GetAllEventHandler();
    }

    public class EventHandlerRepository : RepositoryBase, IEventHandlerRepository
    {
        public EventHandlerRepository(ITransactionProvider transactionProvider) : base(transactionProvider)
        {
        }

        public IQueryable<EventHandler> GetAllEventHandler()
        {
            return CurrentSession.Query<EventHandler>();
        }
    }
}
