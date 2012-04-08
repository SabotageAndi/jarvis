using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jarvis.client.common.Triggers.FileSystemTrigger
{
    public class FileSystemTriggerConfigElementCollection :  ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new FileSystemTriggerConfigElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((FileSystemTriggerConfigElement) element).Path;
        }
    }
}
