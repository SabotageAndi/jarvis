using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using jarvis.common.dtos;
using jarvis.common.dtos.Actionhandling;

namespace jarvis.client.common.Actions
{
    [ServiceContract]
    
    public interface IActionService
    {
        [OperationContract]
        [WebInvoke(UriTemplate = "execute", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        ActionResultDto Execute(ActionDto actionDto);

        [OperationContract]
        [WebGet(UriTemplate = "isExecuting", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        ResultDto<Boolean> IsExecuting();
    }

    [ServiceContract]
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, AddressFilterMode=AddressFilterMode.Any)]
    public class ActionService //: IActionService
    {
        private readonly IActionRegistry _actionRegistry;
        private bool _isExecuting;

        public ActionService(IActionRegistry actionRegistry)
        {
            _actionRegistry = actionRegistry;
            _isExecuting = false;
        }

        [OperationContract]
        [WebInvoke(UriTemplate = "execute", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        public ActionResultDto Execute(ActionDto actionDto)
        {
            _isExecuting = true;
            try
            {
                var actionHandler = _actionRegistry.GetActionHandler(actionDto.ActionGroup);

                if (!actionHandler.CanHandleAction(actionDto))
                {
                    return null;
                }

                return actionHandler.DoAction(actionDto);
            }
            finally
            {
                _isExecuting = false;
            }

        }

        [OperationContract]
        [WebGet(UriTemplate = "isExecuting", ResponseFormat = WebMessageFormat.Json, RequestFormat = WebMessageFormat.Json)]
        public ResultDto<bool> IsExecuting()
        {
            return new ResultDto<bool>(_isExecuting);
        }
    }
}
