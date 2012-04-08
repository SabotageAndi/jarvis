using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jarvis.client.common.Configuration
{
    public class JarvisClientConfigurationSection : ConfigurationSection
    {
        public JarvisClientConfigurationSection()
        {
        }

        public const string SectionName = "client";

        [ConfigurationProperty("ServerUrl", IsRequired = true)]
        public JarvisConfigElement ServerUrl { get { return (JarvisConfigElement)this["ServerUrl"]; } set { this["ServerUrl"] = value; } }

        [ConfigurationProperty("TriggerService", IsRequired = true)]
        public JarvisConfigElement TriggerService { get { return (JarvisConfigElement)this["TriggerService"]; } set { this["TriggerService"] = value; } }

        [ConfigurationProperty("ClientService", IsRequired = true)]
        public JarvisConfigElement ClientService { get { return (JarvisConfigElement)this["ClientService"]; } set { this["ClientService"] = value; } }

        [ConfigurationProperty("ClientId", IsRequired = false)]
        public JarvisConfigElement ClientId { get { return (JarvisConfigElement)this["ClientId"]; } set { this["ClientId"] = value; } }
    }
}
