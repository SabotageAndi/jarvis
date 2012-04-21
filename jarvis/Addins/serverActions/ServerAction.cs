using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jarvis.common.dtos.Actionhandling;
using jarvis.server.repositories;

namespace jarvis.addins.serverActions
{
    public abstract class ServerAction
    {
        public IClientRepository ClientRepository { get; set; }

        public abstract string ActionGroup { get; }

        public abstract ActionResultDto Execute(ActionDto actionDto);

        public abstract bool CanExecute(ActionDto actionDto);
    }
}
