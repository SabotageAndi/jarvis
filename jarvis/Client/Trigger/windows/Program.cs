using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jarvis.client.trigger.common;
using jarvis.common.domain;
using jarvis.common.dtos;

namespace windows
{
    class Program
    {
        private static TriggerLogic _triggerLogic;

        static void Main(string[] args)
        {
            _triggerLogic = new TriggerLogic();

            var fileSystemWatcher = new FileSystemWatcher(@"C:\temp");
            fileSystemWatcher.Created += fileSystemWatcher_Created;

            fileSystemWatcher.IncludeSubdirectories = true;
            fileSystemWatcher.EnableRaisingEvents = true;

            Console.ReadLine();
        }

        static void fileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {
            _triggerLogic.trigger(new EventDto()
                                      {
                                          EventGroupTypes = EventGroupTypes.Filesystem,
                                          EventType = EventType.Add,
                                          TriggeredDate = DateTime.UtcNow
                                      });
            Console.WriteLine(e.FullPath);
        }
    }
}
