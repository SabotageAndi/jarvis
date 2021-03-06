﻿// J.A.R.V.I.S. - Just A Rather Very Intelligent System
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
using jarvis.common.logic;

namespace jarvis.client.common.Configuration
{
    public class JarvisClientConfigurationSection : ConfigurationSection
    {
        public const string SectionName = "client";

        [ConfigurationProperty("ServerUrl", IsRequired = true)]
        public JarvisConfigElement ServerUrl
        {
            get { return (JarvisConfigElement) this["ServerUrl"]; }
            set { this["ServerUrl"] = value; }
        }

        [ConfigurationProperty("ClientId", IsRequired = false)]
        public JarvisConfigElement ClientId
        {
            get { return (JarvisConfigElement) this["ClientId"]; }
            set { this["ClientId"] = value; }
        }

        [ConfigurationProperty("LocalPort", IsRequired = true)]
        public JarvisConfigElement LocalPort
        {
            get { return (JarvisConfigElement) this["LocalPort"]; }
            set { this["LocalPort"] = value; }
        }

        [ConfigurationProperty("Name", IsRequired = true)]
        public JarvisConfigElement Name
        {
            get { return (JarvisConfigElement) this["Name"]; }
            set { this["Name"] = value; }
        }

        [ConfigurationProperty("AddinPath", IsRequired = true)]
        public JarvisConfigElement AddinPath
        {
            get { return (JarvisConfigElement) this["AddinPath"]; }
        }
    }
}