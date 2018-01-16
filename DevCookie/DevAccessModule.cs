using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;

namespace DevCookie
{
    public class DevAccessModule : Module
    {
        public static string SecretToken;
        public static int CookieExpiryInDays;

        private readonly bool _useAsGlobalAuthFilter;

        // todo: add overload for specifying appSetting key that holds the token
        // todo: add supportSslOffloading flag here so that the if (request.IsSecureConnection) in DevAccessChecker can handle the header from SLB
        public DevAccessModule(string secretToken, int cookieExpiryInDays = 1, bool useAsGlobalAuthFilter = false)
        {
            SecretToken = secretToken;
            CookieExpiryInDays = cookieExpiryInDays;
            _useAsGlobalAuthFilter = useAsGlobalAuthFilter;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DevAccessChecker>()
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();

            builder.RegisterFilterProvider();   // todo: end-to-end test! this line is required

            builder.Register(c => new DevAccessQueryStringToCookieFilter())
                .AsAuthorizationFilterFor<Controller>(order: -100)  // order is critical for devcookie UX: other filters will come first without this and the querystring wont work
                .PropertiesAutowired()
                .InstancePerLifetimeScope();

            if (_useAsGlobalAuthFilter)
            {
                builder.Register(c => new DevAccessAuthorizeAttribute())
                    .AsAuthorizationFilterFor<Controller>()
                    .PropertiesAutowired()
                    .InstancePerLifetimeScope();

                // todo: add wiring for webapi controllers?
            }
        }
    }

    public class DevAccessQueryStringToCookieFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            DevAccessChecker.ReturnCookieIfQueryStringPresent(filterContext.HttpContext.Request, filterContext.HttpContext.Response);
        }
    }
}