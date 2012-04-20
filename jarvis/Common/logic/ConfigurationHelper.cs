using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;

namespace jarvis.common.logic
{
    public static class ConfigurationHelper
    {

        public static Configuration GetAssemblyConfiguration()
        {
            var assembly = Assembly.GetCallingAssembly();
            var config = new ExeConfigurationFileMap();
            
            return ConfigurationManager.OpenExeConfiguration(assembly.Location);
        }
    }
}
