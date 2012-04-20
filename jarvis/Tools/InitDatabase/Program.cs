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

using NHibernate;
using NHibernate.Tool.hbm2ddl;
using jarvis.common.domain;
using jarvis.server.configuration;
using jarvis.server.entities.Eventhandling;
using jarvis.server.entities.Management;
using jarvis.server.entities.Workflow;

namespace jarvis.tools.initDatabase
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var configuration = new PostgreSqlConfiguration().GetConfiguration();
            var sessionFactory = configuration.BuildSessionFactory();

            using (ISession session = sessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    new SchemaExport(configuration.BuildConfiguration()).Create(false, true);

                    CreateFolders(session);
                    CreateUsers(session);

                    CreateEventHandler(session);
                    CreateFileCreateEventHandler(session);

                    transaction.Commit();
                }
            }
        }

        private static void CreateEventHandler(ISession session)
        {
            var workflow = new DefinedWorkflow();
            workflow.Name = "Test";
            session.SaveOrUpdate(workflow);

            var definedTask = new DefinedTask();
            definedTask.Name = "Test";
            definedTask.RunCode =
                @"foreach(var parameter in parameters)
                                    {
                                        System.Console.WriteLine(""{0}:{1}:{2}"", parameter.Category, parameter.Name, parameter.Value);
                                    }

                                    return 0;";
            session.SaveOrUpdate(definedTask);

            var definedWorkflowStep = new DefinedWorkflowStep();
            definedWorkflowStep.DefinedTask = definedTask;
            definedWorkflowStep.DefinedWorkflow = workflow;
            session.SaveOrUpdate(definedWorkflowStep);

            var nextWorkflowStep = new DefinedNextWorkflowStep();
            nextWorkflowStep.DefinedWorkflow = workflow;
            nextWorkflowStep.NextStep = definedWorkflowStep;
            nextWorkflowStep.PreviousStep = null;
            session.SaveOrUpdate(nextWorkflowStep);

            var lastWorkflowStep = new DefinedNextWorkflowStep();
            lastWorkflowStep.DefinedWorkflow = workflow;
            lastWorkflowStep.PreviousStep = definedWorkflowStep;
            lastWorkflowStep.NextStep = null;
            session.SaveOrUpdate(lastWorkflowStep);

            var defaultEventHandler = new EventHandler();
            defaultEventHandler.DefinedWorkflow = workflow;

            session.SaveOrUpdate(defaultEventHandler);
        }

        private static void CreateFileCreateEventHandler(ISession session)
        {
            var workflow = new DefinedWorkflow();
            workflow.Name = "DeleteFileOnCreate";
            session.SaveOrUpdate(workflow);

            var definedTask = new DefinedTask();
            definedTask.Name = "Test";
            definedTask.RunCode =
                @"foreach(var parameter in parameters)
                                    {
                                        System.Console.WriteLine(""{0}:{1}:{2}"", parameter.Category, parameter.Name, parameter.Value);
                                    }

                                    string hostname = parameters.Where(p => p.Category == ""EventParameter"" && p.Name == ""Client"").Single().Value;
                                    string path = parameters.Where(p => p.Category == ""EventParameter"" && p.Name == ""Path"").Single().Value;
                                    string file = parameters.Where(p => p.Category == ""EventParameter"" && p.Name == ""Filename"").Single().Value;

                                    string filePath = System.IO.Path.Combine(path, file);
                                    
                                    System.Console.WriteLine(hostname);
                                    System.Console.WriteLine(filePath);

                                    File.Delete(hostname, filePath);

                                    return 0;";
            session.SaveOrUpdate(definedTask);


            var definedWorkflowStep = new DefinedWorkflowStep();
            definedWorkflowStep.DefinedTask = definedTask;
            definedWorkflowStep.DefinedWorkflow = workflow;
            session.SaveOrUpdate(definedWorkflowStep);

            var nextWorkflowStep = new DefinedNextWorkflowStep();
            nextWorkflowStep.DefinedWorkflow = workflow;
            nextWorkflowStep.NextStep = definedWorkflowStep;
            nextWorkflowStep.PreviousStep = null;
            session.SaveOrUpdate(nextWorkflowStep);

            var lastWorkflowStep = new DefinedNextWorkflowStep();
            lastWorkflowStep.DefinedWorkflow = workflow;
            lastWorkflowStep.PreviousStep = definedWorkflowStep;
            lastWorkflowStep.NextStep = null;
            session.SaveOrUpdate(lastWorkflowStep);

            var defaultEventHandler = new EventHandler();
            defaultEventHandler.DefinedWorkflow = workflow;
            defaultEventHandler.EventGroupTypes = EventGroupTypes.Filesystem;
            defaultEventHandler.EventType = EventType.Add;

            session.SaveOrUpdate(defaultEventHandler);
        }

        private static void CreateUsers(ISession session)
        {
            var user = new User();
            user.Username = "andreas";
            user.Password = "123";

            session.SaveOrUpdate(user);
        }

        private static void CreateFolders(ISession session)
        {
            var rootFolder = new Folder();
            rootFolder.Name = "/";

            session.SaveOrUpdate(rootFolder);
        }
    }
}