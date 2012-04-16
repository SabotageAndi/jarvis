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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using jarvis.client.common.ServiceClients;
using jarvis.common.domain;
using jarvis.common.dtos.Eventhandling;
using jarvis.common.dtos.Eventhandling.Parameter;

namespace jarvis.client.common.Triggers.FileSystemTrigger
{
    public class FileSystemTrigger : Trigger
    {
        private readonly IConfiguration _configuration;
        private readonly ITriggerService _triggerService;

        public FileSystemTrigger(IConfiguration configuration, ITriggerService triggerService)
        {
            _configuration = configuration;
            _triggerService = triggerService;
            FileSystemWatchers = new List<FileSystemWatcher>();
        }

        public List<FileSystemWatcher> FileSystemWatchers { get; set; }

        public override bool IsEnabled
        {
            get { return _configuration.FileSystemTriggerEnabled; }
            set { _configuration.FileSystemTriggerEnabled = value; }
        }

        public override void init()
        {
            foreach (var fileSystemTriggerConfigElement in GetPathsToWatch())
            {
                var fileSystemWatcher = new FileSystemWatcher();
                fileSystemWatcher.IncludeSubdirectories = true;
                fileSystemWatcher.Path = fileSystemTriggerConfigElement.Path;
                fileSystemWatcher.Filter = fileSystemTriggerConfigElement.FileFilter;

                if (fileSystemTriggerConfigElement.OnChange)
                {
                    fileSystemWatcher.Changed += fileSystemWatcher_Changed;
                }

                if (fileSystemTriggerConfigElement.OnCreate)
                {
                    fileSystemWatcher.Created += fileSystemWatcher_Changed;
                }

                if (fileSystemTriggerConfigElement.OnDelete)
                {
                    fileSystemWatcher.Deleted += fileSystemWatcher_Changed;
                }

                if (fileSystemTriggerConfigElement.OnRename)
                {
                    fileSystemWatcher.Renamed += fileSystemWatcher_Changed;
                }

                FileSystemWatchers.Add(fileSystemWatcher);
            }
        }

        public override void deinit()
        {
            foreach (var fileSystemWatcher in FileSystemWatchers)
            {
                fileSystemWatcher.EnableRaisingEvents = false;
                fileSystemWatcher.Dispose();
            }

            FileSystemWatchers = new List<FileSystemWatcher>();
        }

        private void fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
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


            _triggerService.EventHappend(new EventDto()
                                             {
                                                 EventGroupTypes = EventGroupTypes.Filesystem,
                                                 EventType = eventType,
                                                 TriggeredDate = DateTime.UtcNow,
                                                 Data = JsonParser.Serializer.Serialize(new FileEventParameterDto
                                                                                            {
                                                                                                Filename = e.Name,
                                                                                                Path = Path.GetDirectoryName(e.FullPath)
                                                                                            })
                                             });
        }

        public override void run()
        {
            if (IsEnabled)
            {
                foreach (var fileSystemWatcher in FileSystemWatchers)
                {
                    fileSystemWatcher.EnableRaisingEvents = true;
                }
            }
        }

        private List<FileSystemTriggerConfigElement> GetPathsToWatch()
        {
            var configFileConfig = _configuration as ConfigFileConfiguration;
            if (configFileConfig != null)
            {
                var fileSystemTriggerConfigElementCollection =
                    configFileConfig.FileSystemTriggerConfigurationSection.ConfigElementCollection;

                return fileSystemTriggerConfigElementCollection.Cast<FileSystemTriggerConfigElement>().ToList();
            }

            return new List<FileSystemTriggerConfigElement>();
        }
    }
}