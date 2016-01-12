using System.Linq;
using System.Web.Mvc;

namespace DevCookie
{
    public class DevCookieAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        private string DevCookieName = "devaccess";
        private string DevCookieValue = "1234";

        public void OnAuthorization(AuthorizationContext filterContext)
        {
            // if the cookie value matches the specified one then OK
            if (filterContext.HttpContext.Request.Cookies.AllKeys.Contains(DevCookieName)
                && filterContext.HttpContext.Request.Cookies.Get(DevCookieName).Value == DevCookieValue)
            {
                return; // authorised!
            }

            // else return 404
            filterContext.Result = new HttpNotFoundResult();
        }
    }
}