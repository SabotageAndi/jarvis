using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jarvis.client.common.Configuration;

namespace jarvis.client.common
{
    public interface IConfiguration
    {
        string ServerUrl { get; }
        string ClientService { get; }
        int? ClientId { get; set; }
        void Save();
    }

    public class ConfigFileConfiguration : IConfiguration
    {
        private System.Configuration.Configuration _configuration;

        private System.Configuration.Configuration Configuration
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

        private JarvisClientConfigurationSection JarvisClientConfigurationSection
        {
            get { return Configuration.GetSection("client") as JarvisClientConfigurationSection; }
        }

        public string ServerUrl
        {
            get { return JarvisClientConfigurationSection.ServerUrl.Value; }
        }

        public string ClientService
        {
            get { return JarvisClientConfigurationSection.ClientService.Value; }
        }

        public void Save()
        {
            JarvisClientConfigurationSection.SectionInformation.ForceSave = true;

            Configuration.Save(ConfigurationSaveMode.Full);
        }

        public int? ClientId
        {
            get
            {
                if (String.IsNullOrEmpty(JarvisClientConfigurationSection.ClientId.Value))
                    return null;
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
