using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using jarvis.common.domain;
using jarvis.common.dtos;
using jarvis.common.dtos.Requests;
using jarvis.server.entities.Management;

namespace jarvis.server.services
{
    public interface IServerStatusService : IService<ServiceStatusRequest>
    {
        ResultDto<Boolean> IsOnline();
    }

    class ServerStatusService : ServiceBase<ServiceStatusRequest>, IServerStatusService
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

        protected override object Run(ServiceStatusRequest request)
        {
            return IsOnline();
        }
   }
}
