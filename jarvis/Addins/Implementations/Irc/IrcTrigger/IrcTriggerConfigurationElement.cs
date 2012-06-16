using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using jarvis.common.logic;

namespace jarvis.addins.irctrigger
{
    public class IrcTriggerConfigurationElement : ConfigurationElement
    {
        [ConfigurationProperty("Network", IsRequired = true)]
        public string Network
        {
            get { return (string)this["Network"]; }
            set { this["Network"] = value; }
        }


        [ConfigurationProperty("Nickname", IsRequired = true)]
        public string Nickname
        {
            get { return (string)this["Nickname"]; }
            set { this["Nickname"] = value; }
        }

        [ConfigurationProperty("Username", IsRequired = true)]
        public string Username
        {
            get { return (string)this["Username"]; }
            set { this["Username"] = value; }
        }

        [ConfigurationProperty("Realname", IsRequired = true)]
        public string Realname
        {
            get { return (string)this["Realname"]; }
            set { this["Realname"] = value; }
        }

        [ConfigurationProperty("Password", IsRequired = true)]
        public string Password
        {
            get { return (string)this["Password"]; }
            set { this["Password"] = value; }
        }

        [ConfigurationProperty("Channels", IsRequired = true)]
        public string Channels
        {
            get { return (string)this["Channels"]; }
            set { this["Channels"] = value; }
        }
    }
}
