using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using log4net;

namespace jarvis.server.services
{
    public class Global : System.Web.HttpApplication
    {
        private ILog log = LogManager.GetLogger(typeof (Global));

        protected void Application_Start(object sender, EventArgs e)
        {
            log.Info("App start");
            Bootstrapper.init();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            log.Info("session start");
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            log.Error(e);
        }

        protected void Session_End(object sender, EventArgs e)
        {
            log.Info("session end");
        }

        protected void Application_End(object sender, EventArgs e)
        {
            log.Info("App stop");
        }
    }
}