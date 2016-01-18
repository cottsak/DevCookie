using System.Web.Mvc;

namespace DevCookie
{
    public class DevCookieAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        public IDevAccessChecker  Checker { get; set; }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Checker.UserHasDevAccess())
                return; // authorised!

            if (DevAccessChecker.QueryStringIsValidAndCookieCreated(filterContext.HttpContext.Request, filterContext.HttpContext.Response))
                return; // authorised!

            // else return 404 (returning 401 is potentially a security risk)
            filterContext.Result = new HttpNotFoundResult();
        }
    }
}