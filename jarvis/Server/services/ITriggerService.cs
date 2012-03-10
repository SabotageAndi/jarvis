using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using jarvis.common.dtos;

namespace jarvis.server.services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "ITriggerService" in both code and config file together.
    [ServiceContract]
    public interface ITriggerService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "/trigger/", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        void EventHappend(EventDto eventDto);

    }
}
