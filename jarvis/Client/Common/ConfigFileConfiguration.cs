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

using System;
using System.Configuration;
using jarvis.client.common.Configuration;

namespace jarvis.client.common
{
    public interface IConfiguration
    {
        int LocalPort { get; }
        string ServerUrl { get; }
        int? ClientId { get; set; }
        string Name { get; }
        string AddinPath { get; }
        System.Configuration.Configuration Configuration { get; }

        void Save();
    }

    public class ConfigFileConfiguration : IConfiguration
    {
        private System.Configuration.Configuration _configuration;

        internal JarvisClientConfigurationSection JarvisClientConfigurationSection
        {
            get { return Configuration.GetSection(JarvisClientConfigurationSection.SectionName) as JarvisClientConfigurationSection; }
        }

        public System.Configuration.Configuration Configuration
        {
            get
            {
                if (_configuration == null)
                {
#if DEBUG
                    string applicationName = Environment.GetCommandLineArgs()[0];
#else
                    string applicationName = Environment.GetCommandLineArgs()[0]+ ".exe";
#endif

                    string exePath = System.IO.Path.Combine(Environment.CurrentDirectory, applicationName);

                    _configuration = ConfigurationManager.OpenExeConfiguration(exePath);
                }

                return _configuration;
            }
        }

        public int LocalPort
        {
            get { return Convert.ToInt32(JarvisClientConfigurationSection.LocalPort.Value); }
        }

        public string ServerUrl
        {
            get { return JarvisClientConfigurationSection.ServerUrl.Value; }
        }

        public string AddinPath
        {
            get { return JarvisClientConfigurationSection.AddinPath.Value; }
        }

        public void Save()
        {
            JarvisClientConfigurationSection.SectionInformation.ForceSave = true;

            Configuration.Save(ConfigurationSaveMode.Full);
        }

        public string Name
        {
            get { return JarvisClientConfigurationSection.Name.Value; }
        }

        public int? ClientId
        {
            get
            {
                if (String.IsNullOrEmpty(JarvisClientConfigurationSection.ClientId.Value))
                {
                    return null;
                }
                return Convert.ToInt32(JarvisClientConfigurationSection.ClientId.Value);
            }
            set
            {
                if (value == null)
                {
                    JarvisClientConfigurationSection.ClientId.Value = String.Empty;
                    return;
                }
                JarvisClientConfigurationSection.ClientId.Value = Convert.ToString(value);
            }
        }
    }
}