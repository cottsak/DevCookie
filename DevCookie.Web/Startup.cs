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
//#if DEBUG
//            app.UseForcedHttps(44300);  // IIS Express
//#else
//            app.UseForcedHttps(443);
//#endif
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
