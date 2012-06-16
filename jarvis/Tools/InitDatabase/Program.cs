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
            var configuration = new DatabaseConfiguration().GetConfiguration();
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
                    //CreateIrcResponder(session);

                    CreateKarma(session);

                    transaction.Commit();
                }
            }
        }

        private static void CreateKarma(ISession session)
        {
            CreateKarmaIncreaseDecrease(session);
            CreateKarmaStats(session);
            CreateKarmaKarma(session);
        }

        private static void CreateKarmaStats(ISession session)
        {
            var runcode = @"
                            var channels = parameters.Where(p => p.Category == ""EventParameter"" && p.Name == ""Channels"").Single().Value;
                            string message = parameters.Where(p => p.Category == ""EventParameter"" && p.Name == ""Message"").Single().Value;
                            string hostname = parameters.Where(p => p.Category == ""EventParameter"" && p.Name == ""Client"").Single().Value;

                            var channel = channels.Split(',')[0];

                            if (!message.Contains(""!stats""))
                               return 0;

                            string newMessage = Karma.GetStats();

                            Irc.SendMessage(hostname, channel, newMessage);
                            return 0;";

            CreateSimpleWorkflow(session, "KarmaStats", "KarmaStats", runcode, EventGroupTypes.Irc);
        }

        private static void CreateKarmaKarma(ISession session)
        {
            var runcode = @"
                            var channels = parameters.Where(p => p.Category == ""EventParameter"" && p.Name == ""Channels"").Single().Value;
                            string message = parameters.Where(p => p.Category == ""EventParameter"" && p.Name == ""Message"").Single().Value;
                            string hostname = parameters.Where(p => p.Category == ""EventParameter"" && p.Name == ""Client"").Single().Value;

                            var channel = channels.Split(',')[0];
                            
                            if (!message.Contains(""!karma""))
                               return 0;

                            string newMessage = ""Average karma: "" + Karma.GetKarma().ToString();

                            Irc.SendMessage(hostname, channel, newMessage);
                            return 0;";

            CreateSimpleWorkflow(session, "KarmaStats", "KarmaStats", runcode, EventGroupTypes.Irc);
        }

        private static void CreateKarmaIncreaseDecrease(ISession session)
        {
            var runCode = @"
                                    var channels = parameters.Where(p => p.Category == ""EventParameter"" && p.Name == ""Channels"").Single().Value;
                                    string message = parameters.Where(p => p.Category == ""EventParameter"" && p.Name == ""Message"").Single().Value;
                                    string hostname = parameters.Where(p => p.Category == ""EventParameter"" && p.Name == ""Client"").Single().Value;

                                    var channel = channels.Split(',')[0];

                                    bool increase = false;
                                    bool decrease = false;

                                    if (message.Contains(""++""))
                                        increase = true;
                                    else
                                    {
                                        if (message.Contains(""--""))
                                            decrease = true;
                                    }

                                    if (!increase && !decrease)
                                        return 0;

                                    string key = String.Empty;
                                    int newValue = 0;

                                    if (increase)            
                                    {
                                        key = message.Split(new string[]{""++""}, 2, StringSplitOptions.RemoveEmptyEntries)[0];
                                        newValue = Karma.IncreaseKarma(key);
                                    }

                                    if (decrease)
                                    {
                                        key = message.Split(new string[]{""--""}, 2, StringSplitOptions.RemoveEmptyEntries)[0];
                                        newValue = Karma.DecreaseKarma(key);
                                    }

                                    string newMessage = key + "": "" + newValue.ToString();

                                    Irc.SendMessage(hostname, channel, newMessage);

                                    return 0;
                                    ";
            CreateSimpleWorkflow(session, "ChangeKarma", "ChangeKarma", runCode, EventGroupTypes.Irc);
        }

        private static void CreateSimpleWorkflow(ISession session, string workflowName, string taskName, string runCode, EventGroupTypes eventGroupTypes)
        {
            var workflow = new DefinedWorkflow();
            workflow.Name = workflowName;
            session.SaveOrUpdate(workflow);

            var definedTask = new DefinedTask();
            definedTask.Name = taskName;
            definedTask.RunCode = runCode;
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
            defaultEventHandler.EventGroupTypes = eventGroupTypes;

            session.SaveOrUpdate(defaultEventHandler);
        }

        private static void CreateIrcResponder(ISession session)
        {
            var workflow = new DefinedWorkflow();
            workflow.Name = "IrcResponder";
            session.SaveOrUpdate(workflow);

            var definedTask = new DefinedTask();
            definedTask.Name = "IrcAnswer";
            definedTask.RunCode = @"
                                    var channels = parameters.Where(p => p.Category == ""EventParameter"" && p.Name == ""Channels"").Single().Value;
                                    string message = parameters.Where(p => p.Category == ""EventParameter"" && p.Name == ""Message"").Single().Value;
                                    string hostname = parameters.Where(p => p.Category == ""EventParameter"" && p.Name == ""Client"").Single().Value;

                                    var channel = channels.Split(',')[0];

                                    Irc.SendMessage(hostname, channel, message);

                                    return 0;
                                    ";
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
            defaultEventHandler.EventGroupTypes = EventGroupTypes.Irc;

            session.SaveOrUpdate(defaultEventHandler);
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