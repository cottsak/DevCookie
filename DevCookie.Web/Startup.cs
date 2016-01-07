using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(DevCookie.Web.Startup))]

namespace DevCookie.Web
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
