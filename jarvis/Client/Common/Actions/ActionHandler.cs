using jarvis.common.domain;
using jarvis.common.dtos.Actionhandling;

namespace jarvis.client.common.Actions
{
    public interface IActionHandler
    {
        ActionGroup ActionGroup { get; }
        bool CanHandleAction(ActionDto actionDto);
        ActionResultDto DoAction(ActionDto actionDto);
    }

    public abstract class ActionHandler : IActionHandler
    {
        public abstract ActionGroup ActionGroup { get; }
        public virtual bool CanHandleAction(ActionDto actionDto)
        {
            return true;
        }

        public abstract ActionResultDto DoAction(ActionDto actionDto);
    }
}