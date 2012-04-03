using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jarvis.client.common
{
    public interface IConfiguration
    {
        string ServerUrl { get; }
        string ClientService { get; }
    }

    public class ConfigFileConfiguration : IConfiguration
    {
        public string ServerUrl
        {
            get { return ConfigurationManager.AppSettings.Get("ServerUrl"); }
        }

        public string ClientService
        {
            get { return ConfigurationManager.AppSettings.Get("ClientService"); }
        }
    }
}
