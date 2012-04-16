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
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CSharp;
using RestSharp;
using jarvis.client.common.Actions.ActionCaller;
using jarvis.client.common.ServiceClients;
using jarvis.common.dtos;
using jarvis.common.dtos.Workflow;

namespace jarvis.client.worker
{
    public interface IWorkflowEngine
    {
        bool Do();
    }

    class WorkflowEngine : IWorkflowEngine
    {
        private readonly IWorkflowService _workflowService;
        private readonly IFileAction _fileAction;

        public WorkflowEngine(IWorkflowService workflowService, IFileAction fileAction)
        {
            _workflowService = workflowService;
            _fileAction = fileAction;
        }

        public bool Do()
        {
            var restResponse = _workflowService.GetWorkflowToExecute();

            if (restResponse == null)
            {
                return false;
            }

            ExecuteWorkflow(restResponse);

            return true;
        }

        private void ExecuteWorkflow(RunnedWorkflowDto runnedWorkflowDto)
        {
            ExecuteStepsAfterStep(runnedWorkflowDto, null);
        }

        private void ExecuteStepsAfterStep(RunnedWorkflowDto runnedWorkflowDto, int? currentStepId)
        {
            var nextSteps2Execute = GetStepsAfterStep(runnedWorkflowDto, currentStepId).ToList();

            foreach (var runnedNextWorkflowStepDto in nextSteps2Execute)
            {
                var step2Execute =
                    runnedWorkflowDto.WorkflowSteps.Where(ws => ws.Id == runnedNextWorkflowStepDto.NextStepId).SingleOrDefault();
                if (step2Execute == null)
                {
                    continue;
                }

                ExecuteStep(step2Execute, runnedWorkflowDto.Parameters);
            }

            foreach (var runnedNextWorkflowStepDto in nextSteps2Execute)
            {
                if (runnedNextWorkflowStepDto.NextStepId != null)
                {
                    ExecuteStepsAfterStep(runnedWorkflowDto, runnedNextWorkflowStepDto.NextStepId.Value);
                }
            }
        }

        private void ExecuteStep(RunnedWorkflowStepDto step2Execute, List<ParameterDto> parameters)
        {
            if (step2Execute.RunnedTask != null)
            {
                ExecuteTask(step2Execute.RunnedTask, parameters);
            }
        }

        private void ExecuteTask(RunnedTaskDto runnedTask, List<ParameterDto> parameters)
        {
            var source = GenerateSource(runnedTask);
            var compile = Compile(source);

            if (compile.Errors.HasErrors)
            {
                foreach (var error in compile.Errors)
                {
                    Console.WriteLine((string) error.ToString());
                }
            }
            else
            {
                var result = Run(compile, runnedTask, parameters);
                Console.WriteLine((int) result);
            }
        }

        private int Run(CompilerResults compile, RunnedTaskDto runnedTask, List<ParameterDto> parameters)
        {
            var instance = compile.CompiledAssembly.CreateInstance("jarvis.client.worker." + runnedTask.Name) as CompiledTask;

            instance.Init(_fileAction);

            return instance.run(parameters);
        }

        private CompilerResults Compile(string source)
        {
            var cSharpCodeProvider = new CSharpCodeProvider();
            var cp = new CompilerParameters();
            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add("System.Core.dll");
            cp.ReferencedAssemblies.Add("System.Data.dll");
            cp.ReferencedAssemblies.Add("System.Xml.dll");
            cp.ReferencedAssemblies.Add("mscorlib.dll");
            cp.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            cp.ReferencedAssemblies.Add("jarvis.client.worker.exe");
            cp.ReferencedAssemblies.Add("jarvis.common.dtos.dll");
            cp.ReferencedAssemblies.Add("jarvis.client.common.dll");
            cp.CompilerOptions = "/target:library";
            cp.GenerateExecutable = false;
            cp.GenerateInMemory = true;

            return cSharpCodeProvider.CompileAssemblyFromSource(cp, new string[] { source });
        }

        private string GenerateSource(RunnedTaskDto runnedTask)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("jarvis.client.worker.Template.generated.cs");
            var template = new StreamReader(stream).ReadToEnd();

            return template.Replace("%RUNCODE%", runnedTask.RunCode).Replace("%TASKNAME%", runnedTask.Name);
        }

        private IEnumerable<RunnedNextWorkflowStepDto> GetStepsAfterStep(RunnedWorkflowDto runnedWorkflowDto, int? currentStepId)
        {
            return runnedWorkflowDto.NextWorkflowSteps.Where(nws => nws.PreviousStepId == currentStepId);
        }
    }
}
