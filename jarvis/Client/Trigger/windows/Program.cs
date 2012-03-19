// J.A.R.V.I.S. - Just a really versatile intelligent system
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
using jarvis.common.dtos;

namespace windows
{
    internal class Program
    {
        private static TriggerLogic _triggerLogic;

        private static void Main(string[] args)
        {
            _triggerLogic = new TriggerLogic();

            var fileSystemWatcher = new FileSystemWatcher(@"C:\temp");
            fileSystemWatcher.Created += fileSystemWatcher_Created;

            fileSystemWatcher.IncludeSubdirectories = true;
            fileSystemWatcher.EnableRaisingEvents = true;

            Console.ReadLine();
        }

        private static void fileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            _triggerLogic.trigger(new EventDto()
                                      {
                                          EventGroupTypes = EventGroupTypes.Filesystem,
                                          EventType = EventType.Add,
                                          TriggeredDate = DateTime.UtcNow,
                                          Data = e.FullPath
                                      });
            Console.WriteLine(e.FullPath);
        }
    }
}