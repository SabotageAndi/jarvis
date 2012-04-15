using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace jarvis.client.common.Actions
{
    public class ActionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ActionRegistry>().As<IActionRegistry>().SingleInstance();
            builder.RegisterType<ActionService>().As<ActionService>().SingleInstance();
            builder.RegisterType<ActionServiceHost>().As<IActionServiceHost>().SingleInstance();
        }
    }
}
