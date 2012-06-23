using System.Collections;
using System.Web;

namespace jarvis.server.common
{
    public class WebContext
    {
        private static readonly WebContext _webContext = new WebContext();
        public static WebContext Current
        {
            get { return _webContext; }
        }

        public IDictionary Items
        {
            get
            {
                if (HttpContext.Current == null)
                {
                    return WcfContext.Current.Items;
                }

                return HttpContext.Current.Items;
            }
        }
    }
}