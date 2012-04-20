// J.A.R.V.I.S. - Just A Rather Very Intelligent System
// Copyright (C) 2012 Andreas Willich <sabotageandi@gmail.com>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.IO;
using System.Linq;
using jarvis.addins.actions;
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
            {
                return false;
            }

            if (actionDto.Action == Action.File_Delete)
            {
                return true;
            }

            return false;
        }

        public override ActionResultDto DoAction(ActionDto actionDto)
        {
            switch (actionDto.Action)
            {
                case Action.File_Delete:
                    return DeleteFile(actionDto);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override ActionGroup ActionGroup
        {
            get { return ActionGroup.File; }
        }

        private ActionResultDto DeleteFile(ActionDto actionDto)
        {
            var fileParameter = actionDto.Parameters.Where(p => p.Category == "File" && p.Name == "Path").SingleOrDefault();
            if (fileParameter == null)
            {
                throw new ParameterNotFoundException();
            }

            File.Delete(fileParameter.Value);

            return new ActionResultDto();
        }
    }
}