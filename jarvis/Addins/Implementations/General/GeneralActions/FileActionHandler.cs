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
using Newtonsoft.Json;
using jarvis.addins.actions;
using jarvis.common.dtos;
using jarvis.common.dtos.Actionhandling;

namespace jarvis.addins.generalactions
{
    public interface IFileActionHandler : IActionHandler
    {
    }

    public class FileActionHandler : ActionHandler, IFileActionHandler
    {
        public override bool CanHandleAction(ActionDto actionDto)
        {
            return actionDto.ActionGroup == FileActionConstants.ActionGroup;
        }

        public override ActionResultDto DoAction(ActionDto actionDto)
        {
            switch (actionDto.Action)
            {
                case FileActionConstants.Action_Delete:
                    return DeleteFile(actionDto);
                case FileActionConstants.Action_Copy:
                    return CopyFile(actionDto);
                case FileActionConstants.Action_Create:
                    return CreateFile(actionDto);
                case FileActionConstants.Action_Move:
                    return MoveFile(actionDto);
                case FileActionConstants.Action_Read:
                    return ReadFile(actionDto);
                case FileActionConstants.Action_Write:
                    return WriteFile(actionDto);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private ActionResultDto ReadFile(ActionDto actionDto)
        {
            var fileParameter = GetParameter(actionDto, "File", "Path");

            var data = File.ReadAllBytes(fileParameter.Value);

            return new ActionResultDto() { Data = JsonSerializer.Serialize(data) };
        }

        private ActionResultDto WriteFile(ActionDto actionDto)
        {
            var fileParameter = GetParameter(actionDto, "File", "Path");
            var dataParameter = GetParameter(actionDto, "File", "Data");

            var data = JsonDeserializer.Deserialize<byte[]>(new JsonTextReader(new StringReader(dataParameter.Value)));

            File.WriteAllBytes(fileParameter.Value, data);

            return new ActionResultDto();
        }

        private ActionResultDto CreateFile(ActionDto actionDto)
        {
            var fileParameter = GetParameter(actionDto, "File", "Path");

            File.Create(fileParameter.Value);

            return new ActionResultDto();
        }

        private ActionResultDto CopyFile(ActionDto actionDto)
        {
            var targetPathParameter = GetParameter(actionDto, "File", "TargetPath");
            var sourePathParameter = GetParameter(actionDto, "File", "SourcePath");


            File.Copy(sourePathParameter.Value, targetPathParameter.Value);

            return new ActionResultDto();
        }

        private ActionResultDto MoveFile(ActionDto actionDto)
        {
            var targetPathParameter = GetParameter(actionDto, "File", "TargetPath");
            var sourePathParameter = GetParameter(actionDto, "File", "SourcePath");

            File.Move(sourePathParameter.Value, targetPathParameter.Value);

            return new ActionResultDto();
        }

        public override string ActionGroup
        {
            get { return "File"; }
        }

        private ActionResultDto DeleteFile(ActionDto actionDto)
        {
            var fileParameter = GetParameter(actionDto, "File", "Path");

            File.Delete(fileParameter.Value);

            return new ActionResultDto();
        }
    }
}