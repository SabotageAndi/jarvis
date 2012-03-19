using System.Web.Mvc;

namespace jarvis.server.web.Areas.Events
{
    public class EventsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Events";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Events_default",
                "Events/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
