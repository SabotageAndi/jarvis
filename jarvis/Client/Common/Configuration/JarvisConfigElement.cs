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
    
        [ConfigurationProperty("value", IsRequired = true, IsKey = false)]
        public string Value { get { return (string)this["value"]; } set { this["value"] = value; } }
    }
}
