using System.Web.Mvc;

namespace DevCookie
{
    public class DevAccessAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        public IDevAccessChecker Checker { get; set; }

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (Checker.UserHasDevAccess())
                return; // authorised!

            // else return 404 (returning 401 is potentially a security risk)
            filterContext.Result = new HttpNotFoundResult();
        }
    }
}