// J.A.R.V.I.S. - Just A Rather Very Intelligent System
// Copyright (C) 2012 Andreas Willich <sabotageandi@gmail.com>
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License, or (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
// 
// You should have received a copy of the GNU Affero General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Configuration;

namespace jarvis.client.common.Configuration
{
    public class JarvisClientConfigurationSection : ConfigurationSection
    {
        public const string SectionName = "client";

        public JarvisClientConfigurationSection()
        {
        }

        [ConfigurationProperty("ServerUrl", IsRequired = true)]
        public JarvisConfigElement ServerUrl
        {
            get { return (JarvisConfigElement) this["ServerUrl"]; }
            set { this["ServerUrl"] = value; }
        }

        [ConfigurationProperty("TriggerService", IsRequired = true)]
        public JarvisConfigElement TriggerService
        {
            get { return (JarvisConfigElement) this["TriggerService"]; }
            set { this["TriggerService"] = value; }
        }

        [ConfigurationProperty("ClientService", IsRequired = true)]
        public JarvisConfigElement ClientService
        {
            get { return (JarvisConfigElement) this["ClientService"]; }
            set { this["ClientService"] = value; }
        }

        [ConfigurationProperty("ClientId", IsRequired = false)]
        public JarvisConfigElement ClientId
        {
            get { return (JarvisConfigElement) this["ClientId"]; }
            set { this["ClientId"] = value; }
        }

        [ConfigurationProperty("ServerStatusService", IsRequired = true)]
        public JarvisConfigElement ServerStatusService
        {
            get { return (JarvisConfigElement)this["ServerStatusService"]; }
            set { this["ServerStatusService"] = value; }
        }

        [ConfigurationProperty("LocalPort", IsRequired = true)]
        public JarvisConfigElement LocalPort
        {
            get { return (JarvisConfigElement) this["LocalPort"]; }
            set { this["LocalPort"] = value; }
        }
    }
}