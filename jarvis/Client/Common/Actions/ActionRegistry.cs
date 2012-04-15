using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using jarvis.common.domain;

namespace jarvis.client.common.Actions
{
    public interface IActionRegistry
    {
        void RegisterActionHandler(ActionHandler actionHandler);
        ActionHandler GetActionHandler(ActionGroup actionGroup);
    }

    public class ActionRegistry : IActionRegistry
    {
        public ActionRegistry()
        {
            ActionHandlers = new Dictionary<ActionGroup, ActionHandler>();
        }

        private Dictionary<ActionGroup, ActionHandler> ActionHandlers { get; set; }

        public void RegisterActionHandler(ActionHandler actionHandler)
        {
            if (ActionHandlers.ContainsKey(actionHandler.ActionGroup))
                throw new ActionHandlerAlreadyRegisteredException();

            ActionHandlers.Add(actionHandler.ActionGroup, actionHandler);
        }

        public ActionHandler GetActionHandler(ActionGroup actionGroup)
        {
            if (!ActionHandlers.ContainsKey(actionGroup))
                throw new ActionHandlerNotFoundException();

            return ActionHandlers[actionGroup];
        }
    }

    public class ActionHandlerNotFoundException : Exception
    {
    }

    public class ActionHandlerAlreadyRegisteredException : Exception
    {
    }

    internal class ParameterNotFoundException : Exception
    {
        
    }
}
