using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
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
            app.RedirectToHttps(44300);     // <- post 44300 is for local iisexpress dev
            //app.UseForcedHttps(44300);


            //#if DEBUG
            //            GlobalFilters.Filters.Add(new HttpsAllTheThings(44300), -100);  // IIS Express; -100 means before all other global filters
            //            // todo: same for webapi
            //#else
            //            GlobalFilters.Filters.Add(new HttpsAllTheThings(), -100);
            //#endif

            GlobalFilters.Filters.Add(new DevCookieAuthorizeAttribute());

            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }

    public static class RedirectToHTTPSExtensions
    {
        public static void RedirectToHttps(this IAppBuilder appBuilder, int? sslPort)
        {
            appBuilder.Use((context, next) =>
            {
                if (context.Request.Uri.Scheme == "http")
                {
                    context.Response.Redirect(string.Format("https://{0}{1}{2}",
                        context.Request.Uri.Host,
                        sslPort.HasValue ? ":" + sslPort.Value : string.Empty,
                        context.Request.Uri.PathAndQuery));

                    // end the pipeline
                    return Task.FromResult(0);
                }

                return next.Invoke();
            });
        }
    }
}
