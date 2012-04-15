using System;
using System.IO;
using System.Linq;
using jarvis.common.domain;
using jarvis.common.dtos.Actionhandling;
using Action = jarvis.common.domain.Action;

namespace jarvis.client.common.Actions.ActionHandlers
{
    public interface IFileActionHandler : IActionHandler
    {
    }

    public class FileActionHandler : ActionHandler, IFileActionHandler
    {
        public override bool CanHandleAction(ActionDto actionDto)
        {
            if (actionDto.ActionGroup != ActionGroup.File)
                return false;

            if (actionDto.Action == Action.File_Delete)
                return true;

            return false;
        }

        public override ActionResultDto DoAction(ActionDto actionDto)
        {
            switch (actionDto.Action)
            {
                case Action.File_Delete:
                    return DeleteFile(actionDto);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private ActionResultDto DeleteFile(ActionDto actionDto)
        {
            var fileParameter = actionDto.Parameters.Where(p => p.Category == "File" && p.Name == "Path").SingleOrDefault();
            if (fileParameter == null)
                throw new ParameterNotFoundException();

            File.Delete(fileParameter.Value);

            return new ActionResultDto();
        }

        public override ActionGroup ActionGroup
        {
            get { return ActionGroup.File;}
        }
    }
}