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
using System.IO;
using jarvis.client.trigger.common;
using jarvis.common.domain;
using jarvis.common.dtos.Eventhandling;
using jarvis.common.dtos.Eventhandling.Parameter;

namespace windows
{
    internal class Program
    {
        private static TriggerLogic _triggerLogic;

        private static void Main(string[] args)
        {
            _triggerLogic = new TriggerLogic();

            var fileSystemWatcher = new FileSystemWatcher(@"C:\temp");
            fileSystemWatcher.Created += fileSystemWatcher_EventHandler;
            fileSystemWatcher.Changed += fileSystemWatcher_EventHandler;
            fileSystemWatcher.Deleted += fileSystemWatcher_EventHandler;
            fileSystemWatcher.Renamed += fileSystemWatcher_EventHandler;

            fileSystemWatcher.IncludeSubdirectories = true;
            fileSystemWatcher.EnableRaisingEvents = true;

            Console.ReadLine();
        }

        private static void fileSystemWatcher_EventHandler(object sender, FileSystemEventArgs e)
        {
            EventType eventType;
            switch (e.ChangeType)
            {
                case WatcherChangeTypes.Created:
                    eventType = EventType.Add;
                    break;
                case WatcherChangeTypes.Deleted:
                    eventType = EventType.Remove;
                    break;
                case WatcherChangeTypes.Changed:
                    eventType = EventType.Changed;
                    break;
                case WatcherChangeTypes.Renamed:
                    eventType = EventType.Renamed;
                    break;
                default:
                    return;
            }


            _triggerLogic.trigger(new EventDto()
                                      {
                                          EventGroupTypes = EventGroupTypes.Filesystem,
                                          EventType = eventType,
                                          TriggeredDate = DateTime.UtcNow,
                                          Data =
                                              JsonParser.Serializer.Serialize(new FileEventParameterDto
                                                                                  {
                                                                                      Filename = e.Name,
                                                                                      Path = Path.GetDirectoryName(e.FullPath)
                                                                                  })
                                      });
            Console.WriteLine(e.FullPath);
        }
    }
}