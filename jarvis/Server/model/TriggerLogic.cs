using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jarvis.common.dtos;
using jarvis.server.entities;
using jarvis.server.repositories;
using jarvis.server.repositories.Infrastructure;

namespace jarvis.server.model
{
    public interface ITriggerLogic
    {
        void eventRaised(EventDto eventDto);
    }

    public class TriggerLogic : ITriggerLogic
    {
        private readonly ISessionFactory _sessionFactory;
        private readonly ITransactionFactory _transactionFactory;
        private readonly ITriggerRepository _triggerRepository;

        public TriggerLogic(ISessionFactory sessionFactory, ITransactionFactory transactionFactory, ITriggerRepository triggerRepository)
        {
            _sessionFactory = sessionFactory;
            _transactionFactory = transactionFactory;
            _triggerRepository = triggerRepository;
        }

        public void eventRaised(EventDto eventDto)
        {
            using (var session = _sessionFactory.OpenSession())
            {
                using (var transaction = _transactionFactory.BeginTransaction(session))
                {
                    var raisedEvent = new Event()
                                          {
                                              EventGroupType = eventDto.EventGroupTypes,
                                              EventType = eventDto.EventType,
                                              TriggeredDate = eventDto.TriggeredDate
                                          };

                    _triggerRepository.saveTrigger(session, raisedEvent);

                    transaction.Commit();
                }
            }

        }
    }
}
