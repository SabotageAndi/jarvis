using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using jarvis.common.domain;
using jarvis.common.dtos;
using jarvis.server.entities.Management;

namespace jarvis.server.web.services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ServerStatusService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ServerStatusService.svc or ServerStatusService.svc.cs at the Solution Explorer and start debugging.
    public class ServerStatusService : IServerStatusService
    {
        private readonly ServerStatus _serverStatus;

        public ServerStatusService(ServerStatus serverStatus)
        {
            _serverStatus = serverStatus;
        }

        public ResultDto<Boolean> IsOnline()
        {
            return new ResultDto<bool>(_serverStatus.State == State.Running);
        }
    }
}
