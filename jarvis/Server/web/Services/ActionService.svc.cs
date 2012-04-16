using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using jarvis.common.dtos.Actionhandling;
using jarvis.server.model;

namespace jarvis.server.web.services
{
    public class ActionService : IActionService
    {
        private readonly IActionLogic _actionLogic;

        public ActionService(IActionLogic actionLogic)
        {
            _actionLogic = actionLogic;
        }

        public ActionResultDto Execute(ActionDto actionDto)
        {
            return _actionLogic.Execute(actionDto);
        }
    }
}
