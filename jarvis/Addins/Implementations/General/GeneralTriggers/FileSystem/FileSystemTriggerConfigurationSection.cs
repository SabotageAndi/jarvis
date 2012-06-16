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
using jarvis.common.logic;

namespace jarvis.addins.generaltriggers.FileSystem
{
    public class FileSystemTriggerConfigurationSection : ConfigurationSection
    {
        public const string SectionName = "FileSystemTrigger";


        [ConfigurationProperty("IsEnabled", IsRequired = true)]
        public JarvisConfigElement IsEnabled
        {
            get { return (JarvisConfigElement) this["IsEnabled"]; }
            set { this["IsEnabled"] = value; }
        }

        [ConfigurationProperty("Paths", IsDefaultCollection = true, IsRequired = true)]
        [ConfigurationCollection(typeof (FileSystemTriggerConfigElementCollection), AddItemName = "add",
            CollectionType = ConfigurationElementCollectionType.BasicMap)]
        public FileSystemTriggerConfigElementCollection ConfigElementCollection
        {
            get { return (FileSystemTriggerConfigElementCollection) base["Paths"]; }
        }
    }
}