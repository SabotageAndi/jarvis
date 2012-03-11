using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using jarvis.server.model;

namespace jarvis.server.services
{
    public class ServiceModule : Module
    {
        public ServiceModule()
        {
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new ModelModule());

            builder.RegisterType<TriggerService>().Named<object>("jarvis.server.web.Services.TriggerService");

            base.Load(builder);
        }
    }
}