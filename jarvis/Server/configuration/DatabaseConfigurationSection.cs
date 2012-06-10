using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using jarvis.common.logic;

namespace jarvis.server.configuration
{
    public class DatabaseConfigurationSection : ConfigurationSection
    {
        public const string SectionName = "Database";


        [ConfigurationProperty("Type", IsRequired = true)]
        public JarvisConfigElement Type
        {
            get { return (JarvisConfigElement)this["Type"]; }
            set { this["Type"] = value; }
        }

        [ConfigurationProperty("Host", IsRequired = true)]
        public JarvisConfigElement Host
        {
            get { return (JarvisConfigElement)this["Host"]; }
            set { this["Host"] = value; }
        }

        [ConfigurationProperty("Database", IsRequired = true)]
        public JarvisConfigElement Database
        {
            get { return (JarvisConfigElement)this["Database"]; }
            set { this["Database"] = value; }
        }

        [ConfigurationProperty("User", IsRequired = true)]
        public JarvisConfigElement User
        {
            get { return (JarvisConfigElement)this["User"]; }
            set { this["User"] = value; }
        }

        [ConfigurationProperty("Password", IsRequired = true)]
        public JarvisConfigElement Password
        {
            get { return (JarvisConfigElement)this["Password"]; }
            set { this["Password"] = value; }
        }
    }
}
