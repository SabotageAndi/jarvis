﻿// J.A.R.V.I.S. - Just a really versatile intelligent system
// Copyright (C) 2012 Andreas Willich <sabotageandi@gmail.com>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

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

        public TriggerLogic(ISessionFactory sessionFactory, ITransactionFactory transactionFactory,
                            ITriggerRepository triggerRepository)
        {
            _sessionFactory = sessionFactory;
            _transactionFactory = transactionFactory;
            _triggerRepository = triggerRepository;
        }

        #region ITriggerLogic Members

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

        #endregion
    }
}