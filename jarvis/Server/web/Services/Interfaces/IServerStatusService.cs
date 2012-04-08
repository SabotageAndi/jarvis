using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using jarvis.common.dtos;

namespace jarvis.server.web.services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServerStatusService" in both code and config file together.
    [ServiceContract]
    public interface IServerStatusService
    {
        [OperationContract]
        [WebGet(UriTemplate = "server/status", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        ResultDto<Boolean> IsOnline();
   }
}
