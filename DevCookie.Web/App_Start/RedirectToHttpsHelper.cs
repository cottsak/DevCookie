using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

namespace DevCookie.Web
{
    internal static class RedirectToHttpsHelper
    {
        internal static void RedirectToHttps(this IAppBuilder appBuilder, string sslPortAppSettingKey)
        {
            RedirectToHttps(appBuilder, ConfigurationManager.AppSettings[sslPortAppSettingKey] == null ? (int?)null
                : Convert.ToInt32(ConfigurationManager.AppSettings[sslPortAppSettingKey]));
        }

        internal static void RedirectToHttps(this IAppBuilder appBuilder, int? sslPort = null)
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
                // if load banalcer is doing 'ssl offloading' it will add a header indicating that 		
                // the request is SSL secured even tho it's not HTTPS by the time it gets here.
                || string.Equals(request.Headers["X-Forwarded-Proto"], "https", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}