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

using jarvis.server.common.Database;
using jarvis.server.entities.Workflow;

namespace jarvis.server.repositories
{
    public interface IParameterRepository : IRepositoryBase<Parameter>
    {
        Parameter Create(RunnedWorkflow runnedWorkflow, string category, string name, string value);
    }

    public class ParameterRepository : RepositoryBase<Parameter>, IParameterRepository
    {
        public ParameterRepository(ITransactionProvider transactionProvider) : base(transactionProvider)
        {
        }

        public Parameter Create(RunnedWorkflow runnedWorkflow, string category, string name, string value)
        {
            var parameter = Create();
            parameter.RunnedWorkflow = runnedWorkflow;
            parameter.Category = category;
            parameter.Name = name;
            parameter.Value = value;

            return parameter;
        }
    }
}