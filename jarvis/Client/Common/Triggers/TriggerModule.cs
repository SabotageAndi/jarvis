using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace jarvis.client.common.Triggers
{
    public class TriggerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<FileSystemTrigger.FileSystemTrigger>();

            base.Load(builder);


        }
    }
}
