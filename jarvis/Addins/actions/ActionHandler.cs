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
using System.Linq;
using jarvis.common.domain;
using jarvis.common.dtos;
using jarvis.common.dtos.Actionhandling;

namespace jarvis.addins.actions
{
    public interface IActionHandler
    {
        string ActionGroup { get; }
        bool CanHandleAction(ActionDto actionDto);
        ActionResultDto DoAction(ActionDto actionDto);
    }

    public abstract class ActionHandler : IActionHandler
    {
        public abstract string ActionGroup { get; }

        public virtual bool CanHandleAction(ActionDto actionDto)
        {
            return true;
        }

        public abstract ActionResultDto DoAction(ActionDto actionDto);

        protected static ParameterDto GetParameter(ActionDto actionDto, string category, string name)
        {
            var result = actionDto.Parameters.Where(p => p.Category == category && p.Name == name).SingleOrDefault();
            if (result== null)
            {
                throw new ParameterNotFoundException();
            }

            return result;
        }

        protected JarvisJsonSerializer JsonSerializer
        {
            get { return JsonParser.GetJsonSerializer(); }
        }

        protected JarvisJsonSerializer JsonDeserializer
        {
            get { return JsonParser.GetJsonSerializer(); }
        }
    }


    public class ParameterNotFoundException : Exception
    {
    }
}