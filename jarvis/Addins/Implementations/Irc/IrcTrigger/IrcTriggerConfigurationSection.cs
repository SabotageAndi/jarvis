using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using jarvis.common.logic;

namespace jarvis.addins.irctrigger
{
    public class IrcTriggerConfigurationSection : ConfigurationSection
    {
        public const string SectionName = "IrcTrigger";


        [ConfigurationProperty("IsEnabled", IsRequired = true)]
        public JarvisConfigElement IsEnabled
        {
            get { return (JarvisConfigElement)this["IsEnabled"]; }
            set { this["IsEnabled"] = value; }
        }

        [ConfigurationProperty("Networks", IsDefaultCollection = true, IsRequired = true)]
        [ConfigurationCollection(typeof(IrcTriggerConfigElementCollection), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.BasicMap)]
        public IrcTriggerConfigElementCollection ConfigElementCollection
        {
            get { return (IrcTriggerConfigElementCollection)base["Networks"]; }
        }
    }
}
