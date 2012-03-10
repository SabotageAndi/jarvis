using NHibernate;
using jarvis.server.entities;

namespace jarvis.server.repositories
{
    public interface ITriggerRepository
    {
        void saveTrigger(ISession session, Event raisedEvent);
    }

    public class TriggerRepository : ITriggerRepository
    {
        public void saveTrigger(ISession session, Event raisedEvent)
        {
            session.SaveOrUpdate(raisedEvent);
        }
    }
}
