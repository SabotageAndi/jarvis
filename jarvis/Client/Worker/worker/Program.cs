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
using jarvis.common.dtos.Workflow;

namespace jarvis.client.worker
{
    class Program
    {
        static RestClient  _client = new RestClient("http://localhost:5368/Services/WorkflowService.svc");

        static void Main(string[] args)
        {
            Thread.Sleep(30000);
            while(true)
            {
                if (!Do())
                {
                    Thread.Sleep(10000);
                }
            }
        }

        private static bool Do()
        {
            var workflowToExecuteRequest = RestRequestFactory.Create("workflow", Method.GET);

            var restResponse = _client.Execute<RunnedWorkflowDto>(workflowToExecuteRequest);

            if (restResponse == null || restResponse.Data == null)
                return false;

            ExecuteWorkflow(restResponse.Data);

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
                var step2Execute = runnedWorkflowDto.WorkflowSteps.Where(ws => ws.Id == runnedNextWorkflowStepDto.NextStepId).SingleOrDefault();
                if (step2Execute == null)
                    continue;

                ExecuteStep(step2Execute);
            }

            foreach (var runnedNextWorkflowStepDto in nextSteps2Execute)
            {
                if (runnedNextWorkflowStepDto.NextStepId != null)
                    ExecuteStepsAfterStep(runnedWorkflowDto, runnedNextWorkflowStepDto.NextStepId.Value);
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
