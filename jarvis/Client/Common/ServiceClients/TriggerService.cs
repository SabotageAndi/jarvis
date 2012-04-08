// J.A.R.V.I.S. - Just A Rather Very Intelligent System
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

using RestSharp;
using jarvis.common.dtos.Eventhandling;

namespace jarvis.client.common.ServiceClients
{
    public interface ITriggerService
    {
        void EventHappend(EventDto eventDto);
    }

    public class TriggerService : ServiceBase, ITriggerService
    {
        public TriggerService(IJarvisRestClient jarvisRestClient, IConfiguration configuration) : base(jarvisRestClient, configuration)
        {
        }

        protected override string ServiceName
        {
            get { return _configuration.TriggerService; }
        }

        public void EventHappend(EventDto eventDto)
        {
            var eventHappendRequest = JarvisRestClient.CreateRequest("trigger", Method.POST);
            eventHappendRequest.AddBody(eventDto);


            JarvisRestClient.Execute(eventHappendRequest);
        }
    }
}