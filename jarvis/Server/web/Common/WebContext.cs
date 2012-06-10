using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace jarvis.server.web.Common
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