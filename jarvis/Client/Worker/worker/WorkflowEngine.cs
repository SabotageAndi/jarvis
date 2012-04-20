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
using Autofac;
using Microsoft.CSharp;
using jarvis.client.common;
using jarvis.client.common.Actions.ActionCaller;
using jarvis.client.common.ServiceClients;
using jarvis.common.dtos;
using jarvis.common.dtos.Workflow;
using Action = jarvis.addins.actions.Action;

namespace jarvis.client.worker
{
    public interface IWorkflowEngine
    {
        bool Do();
        void AddAction(Action action);
    }

    internal class WorkflowEngine : IWorkflowEngine
    {
        private readonly IWorkflowService _workflowService;
        private readonly IConfiguration _configuration;

        public WorkflowEngine(IWorkflowService workflowService, IConfiguration configuration)
        {
            _workflowService = workflowService;
            _configuration = configuration;
            Actions = new List<Action>();
        }

        protected List<Action> Actions { get; set; }

        public void AddAction(Action action)
        {
            Actions.Add(action);   
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
            var instance = compile.CompiledAssembly.CreateInstance("jarvis.client.worker." + GetClassName(runnedTask)) as CompiledTask;

            Client.Container.InjectProperties(instance);

            var properties = instance.GetType().GetProperties().Where(pi => pi.CanRead && pi.CanWrite);
            foreach (var propertyInfo in properties)
            {
                var value = propertyInfo.GetValue(instance, null);
                Client.Container.InjectProperties(value);
            }

            return instance.run(parameters);
        }

        private CompilerResults Compile(string source)
        {
            var cSharpCodeProvider = new CSharpCodeProvider();
            var compilerParameters = new CompilerParameters();
          
            AddReferenceAssemblies(compilerParameters, Assembly.GetEntryAssembly());

            foreach (var action in Actions)
            {
                var assembly = action.GetType().Assembly;
                var assemblyFilename = Path.Combine(_configuration.AddinPath, assembly.GetName().Name + ".dll");
                
                if (!compilerParameters.ReferencedAssemblies.Contains(assemblyFilename))
                    compilerParameters.ReferencedAssemblies.Add(assemblyFilename);

                AddReferenceAssemblies(compilerParameters, assembly);
            }

            compilerParameters.ReferencedAssemblies.Add(Assembly.GetEntryAssembly().GetName().Name + ".exe");
            compilerParameters.CompilerOptions = "/target:library";
            compilerParameters.GenerateExecutable = false;
            compilerParameters.GenerateInMemory = true;

            return cSharpCodeProvider.CompileAssemblyFromSource(compilerParameters, new string[] {source});
        }

        private static void AddReferenceAssemblies(CompilerParameters compilerParameters, Assembly assembly)
        {
            foreach (var referencedAssembly in assembly.GetReferencedAssemblies())
            {
                var assemblyFilename = referencedAssembly.Name + ".dll";

                if (!compilerParameters.ReferencedAssemblies.Contains(assemblyFilename))
                {
                    compilerParameters.ReferencedAssemblies.Add(assemblyFilename);
                }
            }
        }

        private string GenerateSource(RunnedTaskDto runnedTask)
        {
            var source = AddClassName(SourceTemplate, runnedTask);
            source = AddInjectedPropertyCode(source, runnedTask);
            source = AddRunCode(source, runnedTask);

            return source;
        }

        private string _sourceTemplate;
        private string SourceTemplate
        {
            get
            {
                if (_sourceTemplate == null)
                {
                    _sourceTemplate = GetSourceTemplate();
                }
                return _sourceTemplate;
            }
        }

        private static string GetSourceTemplate()
        {
            var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("jarvis.client.worker.Template.generated.cs");
            var template = new StreamReader(stream).ReadToEnd();
            return template;
        }

        private string AddRunCode(string source, RunnedTaskDto runnedTask)
        {
            return source.Replace("%RUNCODE%", runnedTask.RunCode);
        }

        private string AddInjectedPropertyCode(string source, RunnedTaskDto runnedTask)
        {
            var properties = String.Join(Environment.NewLine, (from a in Actions
                             select "public " + a.GetType().FullName + " " + a.PropertyName + " { get; set; }"));

            source = source.Replace("%INJECTEDPROPERTIES%", properties);

            return source;
        }

        private string AddClassName(string source, RunnedTaskDto runnedTask)
        {
            return source.Replace("%TASKNAME%", GetClassName(runnedTask));
        }

        private static string GetClassName(RunnedTaskDto runnedTask)
        {
            return runnedTask.Name;
        }

        private IEnumerable<RunnedNextWorkflowStepDto> GetStepsAfterStep(RunnedWorkflowDto runnedWorkflowDto, int? currentStepId)
        {
            return runnedWorkflowDto.NextWorkflowSteps.Where(nws => nws.PreviousStepId == currentStepId);
        }
    }
}