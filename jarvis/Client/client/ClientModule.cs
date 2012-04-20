using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autofac;
using jarvis.addins.actions;
using jarvis.client.common;

namespace jarvis.client
{
    public class ClientModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule<ActionModule>();

            builder.RegisterModule<CommonModule>();
        }
    }
}
