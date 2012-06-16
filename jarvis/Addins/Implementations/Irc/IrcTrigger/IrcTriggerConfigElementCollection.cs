using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace jarvis.addins.irctrigger
{
    public class IrcTriggerConfigElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new IrcTriggerConfigurationElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IrcTriggerConfigurationElement)element).Network;
        }
    }
}
