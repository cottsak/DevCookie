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
            GlobalFilters.Filters.Add(new HttpsAllTheThings(44300));  // IIS Express
            // todo: same for webapi
#else
            GlobalFilters.Filters.Add(new HttpsAllTheThings());
#endif

            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
