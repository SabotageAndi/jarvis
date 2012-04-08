using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jarvis.client.common.Configuration;

namespace jarvis.client.common.Triggers.FileSystemTrigger
{
    public class FileSystemTriggerConfigurationSection : ConfigurationSection
    {
        public const string SectionName = "FileSystemTrigger"; 


        [ConfigurationProperty("IsEnabled", IsRequired = true)]
        public JarvisConfigElement IsEnabled { get { return (JarvisConfigElement)this["IsEnabled"]; } set { this["IsEnabled"] = value; } }

        [ConfigurationProperty("Paths", IsDefaultCollection = true, IsRequired = true)]
        [ConfigurationCollection(typeof(FileSystemTriggerConfigElementCollection), AddItemName = "add", CollectionType = ConfigurationElementCollectionType.BasicMap)]
        public FileSystemTriggerConfigElementCollection ConfigElementCollection { get { return (FileSystemTriggerConfigElementCollection) base["Paths"]; }}
    }
}
