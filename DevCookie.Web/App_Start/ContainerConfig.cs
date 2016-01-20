using System.Web.Http;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using DevCookie.Web.Controllers;
using Owin;

namespace DevCookie.Web
{
    static class ContainerConfig
    {
        internal static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterAssemblyModules(typeof(ContainerConfig).Assembly);
            // configure DevCookie
            builder.RegisterModule(new DevAccessModule("U4SdMn12dTkLT4aktB75fvdpPcqnmEBc39aufs3QlGo6x2SZYo"));
            return builder.Build();
        }

        internal static IContainer SetupDependencyInjection(IAppBuilder app)
        {
            var container = BuildContainer();

            // OWIN config
            app.UseAutofacMiddleware(container);
            app.UseAutofacMvc();

            // MVC and WebApi wiring for DI
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);
            return container;
        }
    }

    class WebRegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // MVC
            builder.RegisterModule(new AutofacWebTypesModule());
            builder.RegisterControllers(ThisAssembly).PropertiesAutowired();
            builder.RegisterFilterProvider();

            // WebApi
            builder.RegisterApiControllers(ThisAssembly);
            builder.RegisterWebApiFilterProvider(GlobalConfiguration.Configuration);

            // other
            builder.RegisterType<FooService>().AsImplementedInterfaces().InstancePerLifetimeScope();
        }
    }
}