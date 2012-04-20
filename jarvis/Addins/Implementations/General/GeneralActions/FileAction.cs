﻿// J.A.R.V.I.S. - Just A Rather Very Intelligent System
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

using jarvis.client.common.ServiceClients;
using jarvis.common.domain;
using jarvis.common.dtos;
using jarvis.common.dtos.Actionhandling;

namespace jarvis.addins.generalactions
{
    public interface IFileAction
    {
        void Delete(string clientName, string filePath);
    }

    public class FileAction : actions.Action, IFileAction 
    {
        public void Delete(string clientName, string filePath)
        {
            var actionDto = new ActionDto();
            actionDto.ActionGroup = ActionGroup.File;
            actionDto.Action = Action.File_Delete;
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

            ActionService.Execute(actionDto);
        }

        public override string PropertyName
        {
            get { return "File"; }
        }
    }
}