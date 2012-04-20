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

using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Web;
using jarvis.common.dtos.Eventhandling;
using jarvis.common.dtos.Workflow;

namespace jarvis.server.web.services
{
    [ServiceContract]
    internal interface IEventHandlingService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/eventhandler/", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            Method = "GET")]
        List<EventHandlerDto> GetAllEventhandler();

        [OperationContract]
        [WebGet(UriTemplate = "/events/{ticks}", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        List<EventDto> GetAllEventsSince(String ticks);

        [OperationContract]
        [WebInvoke(UriTemplate = "/workflowqueue/", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json,
            Method = "POST")]
        void CreateNewItemInWorkflowQueue(WorkflowQueueDto workflowQueueDto);
    }
}