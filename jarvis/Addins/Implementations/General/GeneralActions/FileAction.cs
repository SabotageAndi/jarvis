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

using System.IO;
using Newtonsoft.Json;
using jarvis.client.common.ServiceClients;
using jarvis.common.domain;
using jarvis.common.dtos;
using jarvis.common.dtos.Actionhandling;

namespace jarvis.addins.generalactions
{
    public interface IFileAction
    {
        void Delete(string clientName, string filePath);
        void Create(string clientName, string filePath);
        void Move(string sourceClientName, string sourceFilePath, string targetClientName, string targetFilePath);
        void Copy(string sourceClientName, string sourceFilePath, string targetClientName, string targetFilePath);
        byte[] Read(string sourceClientName, string filePath);
        void Write(string sourceClientName, string filePath, byte[] data);
    }

    public class FileAction : actions.ClientAction, IFileAction 
    {
        public void Delete(string clientName, string filePath)
        {
            ExecuteFileOperation(clientName, filePath, FileActionConstants.Action_Delete);
        }

        public void Create(string clientName, string filePath)
        {
            ExecuteFileOperation(clientName, filePath, FileActionConstants.Action_Create);
        }

        public void Move(string sourceClientName, string sourceFilePath, string targetClientName, string targetFilePath)
        {
            ExecuteFileOperation(sourceClientName, sourceFilePath, targetClientName, targetFilePath, FileActionConstants.Action_Move);
        }

        public void Copy(string sourceClientName, string sourceFilePath, string targetClientName, string targetFilePath)
        {
            ExecuteFileOperation(sourceClientName, sourceFilePath, targetClientName, targetFilePath, FileActionConstants.Action_Copy);
        }

        public byte[] Read(string sourceClientName, string filePath)
        {
            var result = ExecuteFileOperation(sourceClientName, filePath, FileActionConstants.Action_Read);

            return JsonDeserializer.Deserialize<byte[]>(new JsonTextReader(new StringReader(result.Data)));
        }

        public void Write(string sourceClientName, string filePath, byte[] data)
        {
            var actionDto = CreateActionDto(sourceClientName, filePath, FileActionConstants.Action_Write);
            actionDto.Parameters.Add(new ParameterDto()
                                         {
                                             Category = "File",
                                             Name = "Data",
                                             Value =  JsonSerializer.Serialize(data)
                                         });

            ActionService.Execute(actionDto);
        }

        private ActionResultDto ExecuteFileOperation(string sourceClientName, string sourceFilePath, string action)
        {
            var actionDto = CreateActionDto(sourceClientName, sourceFilePath, action);

            return ActionService.Execute(actionDto);
        }

        private ActionResultDto ExecuteFileOperation(string sourceClientName, string sourceFilePath, string targetClientName, string targetFilePath, string action)
        {
            var actionDto = CreateActionDto(sourceClientName, sourceFilePath, targetClientName, targetFilePath, action);

            return ActionService.Execute(actionDto);
        }

        private ActionDto CreateActionDto(string sourceClientName, string sourceFilePath, string targetClientName, string targetFilePath, string action)
        {
            var actionDto = new ActionDto();
            actionDto.ActionGroup = FileActionConstants.ActionGroup;
            actionDto.Action = action;
            actionDto.Parameters.Add(new ParameterDto()
                                         {
                                             Category = "File",
                                             Name = "SourceClient",
                                             Value = sourceClientName
                                         });
            actionDto.Parameters.Add(new ParameterDto()
            {
                Category = "File",
                Name = "SourcePath",
                Value = sourceFilePath
            });

            actionDto.Parameters.Add(new ParameterDto()
            {
                Category = "File",
                Name = "TargetClient",
                Value = targetClientName
            });
            actionDto.Parameters.Add(new ParameterDto()
            {
                Category = "File",
                Name = "TargetPath",
                Value = targetFilePath
            });
            return actionDto;



        }

        private static ActionDto CreateActionDto(string clientName, string filePath, string action)
        {
            var actionDto = new ActionDto();
            actionDto.ActionGroup = FileActionConstants.ActionGroup;
            actionDto.Action = action;
            actionDto.Parameters.Add(new ParameterDto()
                                         {
                                             Category = "File",
                                             Name = "Path",
                                             Value = filePath
                                         });
            actionDto.Parameters.Add(new ParameterDto()
                                         {
                                             Category = "File",
                                             Name = "Client",
                                             Value = clientName
                                         });
            return actionDto;
        }

        public override string PropertyName
        {
            get { return "File"; }
        }
    }
}