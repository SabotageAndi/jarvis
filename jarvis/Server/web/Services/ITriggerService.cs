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

using System.ServiceModel;
using System.ServiceModel.Web;
using jarvis.common.dtos;

namespace jarvis.server.web.services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITriggerService" in both code and config file together.
    [ServiceContract]
    public interface ITriggerService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/trigger/", RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        void EventHappend(EventDto eventDto);
    }
}