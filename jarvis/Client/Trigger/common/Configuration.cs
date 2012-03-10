using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace jarvis.client.trigger.common
{
    public class Configuration : Module
    {
    

        public Configuration(string hostname)
        {
           
        }

        protected override void Load(ContainerBuilder builder)
        {



            base.Load(builder);
        }

        
    }
}
