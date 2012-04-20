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

namespace jarvis.addins.generaltriggers.FileSystem
{
    public class FileSystemTriggerConfigElement : ConfigurationElement
    {
        [ConfigurationProperty("Path", IsRequired = true, IsKey = true)]
        public string Path
        {
            get { return (string) this["Path"]; }
            set { this["Path"] = value; }
        }

        [ConfigurationProperty("FileFilter", DefaultValue = null)]
        public string FileFilter
        {
            get { return (string) this["FileFilter"]; }
            set { this["FileFilter"] = value; }
        }

        [ConfigurationProperty("OnCreate", DefaultValue = true)]
        public bool OnCreate
        {
            get { return (bool) this["OnCreate"]; }
            set { this["OnCreate"] = value; }
        }

        [ConfigurationProperty("OnDelete", DefaultValue = true)]
        public bool OnDelete
        {
            get { return (bool) this["OnDelete"]; }
            set { this["OnDelete"] = value; }
        }

        [ConfigurationProperty("OnChange", DefaultValue = true)]
        public bool OnChange
        {
            get { return (bool) this["OnChange"]; }
            set { this["OnChange"] = value; }
        }

        [ConfigurationProperty("OnRename", DefaultValue = true)]
        public bool OnRename
        {
            get { return (bool) this["OnRename"]; }
            set { this["OnRename"] = value; }
        }
    }
}