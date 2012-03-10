﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Autofac;
using Autofac.Integration.Wcf;
using log4net;
using log4net.Config;
using log4net.Repository.Hierarchy;

namespace jarvis.server.services
{
    public class Bootstrapper
    {

        public static IContainer Container;

        public static void init()
        {
            XmlConfigurator.Configure();


            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterModule(new ServiceModule());

            Container = containerBuilder.Build();

            AutofacHostFactory.Container = Container;
        }
    }
}