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
            app.RedirectToHttps("ssl-port");

            GlobalFilters.Filters.Add(new DevCookieAuthorizeAttribute());

            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
