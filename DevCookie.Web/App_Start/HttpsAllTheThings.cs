using System;
using System.Web.Mvc;

namespace DevCookie.Web
{
    /// <summary>
    /// Redirect all non-HTTPS traffic to HTTPS (for MVC controllers)
    /// </summary>
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
            var redirectToHttps = $"https://{request.Host}{(_sslPort.HasValue ? ":" + _sslPort.Value : string.Empty)}{request.PathAndQuery}";
            filterContext.Result = new RedirectResult(redirectToHttps);
        }
    }
}