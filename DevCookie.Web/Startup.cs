using System;
using System.Web.Mvc;
using System.Web.Routing;
using DevCookie.Web;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace DevCookie.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
#if DEBUG
            GlobalFilters.Filters.Add(new HttpsAllTheThings(44300), -100);  // IIS Express; -100 means before all other global filters
            // todo: same for webapi
#else
            GlobalFilters.Filters.Add(new HttpsAllTheThings(), -100);
#endif

            GlobalFilters.Filters.Add(new DevCookieAuthorizeAttribute());

            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
