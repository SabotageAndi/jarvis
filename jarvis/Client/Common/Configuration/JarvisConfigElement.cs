using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jarvis.client.common.Configuration
{
    public class JarvisConfigElement : ConfigurationElement
    {
        //[ConfigurationProperty("name", IsRequired = true, IsKey = false)]
        //public string Name { get { return (string)this["name"]; } set { this["name"] = value; } }

        [ConfigurationProperty("value", IsRequired = true, IsKey = false)]
        public string Value { get { return (string)this["value"]; } set { this["value"] = value; } }
    }
}
