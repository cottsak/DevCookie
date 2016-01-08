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
            GlobalFilters.Filters.Add(new HttpsAllTheThings(44300));  // IIS Express
#else
            GlobalFilters.Filters.Add(new HttpsAllTheThings());
#endif

            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }

    public class HttpsAllTheThings : ActionFilterAttribute
    {
        private readonly int? _sslPort;

        public HttpsAllTheThings(int? sslPort = null)
        {
            _sslPort = sslPort;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // if request is HTTPS then OK
            if (filterContext.HttpContext.Request.IsSecureConnection)
            {
                return;
            }   

            // if load banalcer is doing 'ssl offloading' it will add a header indicating that 
            // the request is SSL secured even tho it's not HTTPS by the time it gets here.
            if (string.Equals(filterContext.HttpContext.Request.Headers["X-Forwarded-Proto"], "https",
                StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }

            // if not HTTPS then redirect to HTTPS
            var request = filterContext.RequestContext.HttpContext.Request.Url;
            var redirectToHttps = string.Format("https://{0}{1}{2}",
                request.Host,
                _sslPort.HasValue ? ":" + _sslPort.Value : string.Empty,
                request.PathAndQuery);
            filterContext.Result = new RedirectResult(redirectToHttps);
        }
    }
}
