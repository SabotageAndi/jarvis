// J.A.R.V.I.S. - Just a really versatile intelligent system
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
using jarvis.server.model;

namespace jarvis.server.web.services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "TriggerService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select TriggerService.svc or TriggerService.svc.cs at the Solution Explorer and start debugging.
    public class TriggerService : ITriggerService
    {
        private readonly ITriggerLogic _triggerLogic;

        public TriggerService(ITriggerLogic triggerLogic)
        {
            _triggerLogic = triggerLogic;
        }


        public void EventHappend(EventDto eventDto)
        {
            _triggerLogic.eventRaised(eventDto);
        }
    }
}