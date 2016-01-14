using System;
using System.Configuration;
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
            app.RedirectToHttps("ssl-port");

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

    public static class RedirectToHttpsExtensions
    {
        public static void RedirectToHttps(this IAppBuilder appBuilder, string sslPortAppSettingKey)
        {
            RedirectToHttps(appBuilder,
                ConfigurationManager.AppSettings[sslPortAppSettingKey] == null ? (int?)null
                : Convert.ToInt32(ConfigurationManager.AppSettings[sslPortAppSettingKey])
                );
        }

        public static void RedirectToHttps(this IAppBuilder appBuilder, int? sslPort = null)
        {
            appBuilder.Use((context, next) =>
            {
                if (IsSecure(context.Request))
                    return next.Invoke();

                // connection is not secure, so redirect to HTTPS
                context.Response.Redirect(string.Format("https://{0}{1}{2}",
                    context.Request.Uri.Host,
                    sslPort.HasValue ? ":" + sslPort.Value : string.Empty,
                    context.Request.Uri.PathAndQuery));

                // end the pipeline so nothing is processed insecurely
                return Task.FromResult(0);
            });
        }

        private static bool IsSecure(IOwinRequest request)
        {
            return request.Uri.Scheme == "https"
                || string.Equals(request.Headers["X-Forwarded-Proto"], "https", StringComparison.InvariantCultureIgnoreCase);  // from load balancer
        }
    }
}
