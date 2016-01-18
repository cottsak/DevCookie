using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

namespace DevCookie.Web
{
    internal static class RedirectToHttpsHelper
    {
        internal static void RedirectToHttps(this IAppBuilder appBuilder, string sslPortAppSettingKey, bool supportSslOffloading = false)
        {
            RedirectToHttps(appBuilder, ConfigurationManager.AppSettings[sslPortAppSettingKey] == null ? (int?)null
                : Convert.ToInt32(ConfigurationManager.AppSettings[sslPortAppSettingKey]),
                supportSslOffloading);
        }

        internal static void RedirectToHttps(this IAppBuilder appBuilder, int? sslPort = null, bool supportSslOffloading = false)
        {
            appBuilder.Use((context, next) =>
            {
                if (IsSecure(context.Request, supportSslOffloading))
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

        private static bool IsSecure(IOwinRequest request, bool supportSslOffloading)
        {
            return request.IsSecure
                // if load banalcer is doing 'ssl offloading' it will add a header indicating that 		
                // the request is SSL secured even tho it's not HTTPS by the time it gets here.
                || (supportSslOffloading && string.Equals(request.Headers["X-Forwarded-Proto"], "https", StringComparison.InvariantCultureIgnoreCase));
        }
    }
}