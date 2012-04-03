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
using System.Threading;
using Microsoft.CSharp;
using RestSharp;
using jarvis.client.common;
using jarvis.client.common.ServiceClients;
using jarvis.common.dtos.Workflow;

namespace jarvis.client.worker
{
    internal class Program
    {
        private static readonly JarvisRestClient _client = new JarvisRestClient(){BaseUrl = "http://localhost:5368/Services/WorkflowService.svc"};

        private static void Main(string[] args)
        {
            Thread.Sleep(30000);
            while (true)
            {
                if (!Do())
                {
                    Thread.Sleep(10000);
                }
            }
        }

        private static bool Do()
        {
            var workflowToExecuteRequest = _client.CreateRequest("workflow", Method.GET);

            var restResponse = _client.Execute<RunnedWorkflowDto>(workflowToExecuteRequest);

            if (restResponse == null)
            {
                return false;
            }

            ExecuteWorkflow(restResponse);

            return true;
        }

        private static void ExecuteWorkflow(RunnedWorkflowDto runnedWorkflowDto)
        {
            ExecuteStepsAfterStep(runnedWorkflowDto, null);
        }

        private static void ExecuteStepsAfterStep(RunnedWorkflowDto runnedWorkflowDto, int? currentStepId)
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

                ExecuteStep(step2Execute);
            }

            foreach (var runnedNextWorkflowStepDto in nextSteps2Execute)
            {
                if (runnedNextWorkflowStepDto.NextStepId != null)
                {
                    ExecuteStepsAfterStep(runnedWorkflowDto, runnedNextWorkflowStepDto.NextStepId.Value);
                }
            }
        }

        private static void ExecuteStep(RunnedWorkflowStepDto step2Execute)
        {
            if (step2Execute.RunnedTask != null)
            {
                ExecuteTask(step2Execute.RunnedTask);
            }
        }

        private static void ExecuteTask(RunnedTaskDto runnedTask)
        {
            var source = GenerateSource(runnedTask);
            var compile = Compile(source);

            if (compile.Errors.HasErrors)
            {
                foreach (var error in compile.Errors)
                {
                    Console.WriteLine(error.ToString());
                }
            }
            else
            {
                var result = Run(compile, runnedTask);
                Console.WriteLine(result);
            }
        }

        private static int Run(CompilerResults compile, RunnedTaskDto runnedTask)
        {
            var instance = compile.CompiledAssembly.CreateInstance("jarvis.client.worker." + runnedTask.Name) as ICompiledTask;

            return instance.run();
        }

        private static CompilerResults Compile(string source)
        {
            var cSharpCodeProvider = new CSharpCodeProvider();
            var cp = new CompilerParameters();
            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add("System.Data.dll");
            cp.ReferencedAssemblies.Add("System.Xml.dll");
            cp.ReferencedAssemblies.Add("mscorlib.dll");
            cp.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            cp.ReferencedAssemblies.Add("jarvis.client.worker.exe");
            cp.CompilerOptions = "/target:library";
            cp.GenerateExecutable = false;
            cp.GenerateInMemory = true;

            return cSharpCodeProvider.CompileAssemblyFromSource(cp, new string[] {source});
        }

        private static string GenerateSource(RunnedTaskDto runnedTask)
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("jarvis.client.worker.Template.generated.cs");
            var template = new StreamReader(stream).ReadToEnd();

            return template.Replace("%RUNCODE%", runnedTask.RunCode).Replace("%TASKNAME%", runnedTask.Name);
        }

        private static IEnumerable<RunnedNextWorkflowStepDto> GetStepsAfterStep(RunnedWorkflowDto runnedWorkflowDto, int? currentStepId)
        {
            return runnedWorkflowDto.NextWorkflowSteps.Where(nws => nws.PreviousStepId == currentStepId);
        }
    }

    public interface ICompiledTask
    {
        int run();
    }
}