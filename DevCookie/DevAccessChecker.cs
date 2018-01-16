using System;
using System.Web;

namespace DevCookie
{
    public interface IDevAccessChecker
    {
        bool UserHasDevAccess();
    }

    public class DevAccessChecker : IDevAccessChecker
    {
        internal const string CookieName = "devaccess";
        internal const string QueryStringName = "devaccess";

        private readonly HttpRequestBase _request;

        public DevAccessChecker(HttpRequestBase request)
        {
            _request = request;
        }

        public bool UserHasDevAccess()
        { return CookieIsValid(_request); }

        private static bool CookieIsValid(HttpRequestBase request)
        {
            var cookie = request.Cookies[CookieName];
            return cookie != null && cookie.Value == DevAccessModule.SecretToken;
        }

        internal static void ReturnCookieIfQueryStringPresent(HttpRequestBase request, HttpResponseBase response)
        {
            var authToken = request.QueryString[QueryStringName];
            if (authToken != DevAccessModule.SecretToken)
                return;

            var authCookie = new HttpCookie(CookieName, authToken) { Expires = DateTime.UtcNow.AddDays(DevAccessModule.CookieExpiryInDays) };
            if (request.IsSecureConnection)
                authCookie.Secure = true;       // todo: an integration test for this would be great. it's kinda important
            response.Cookies.Add(authCookie);
        }
    }
}