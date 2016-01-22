# DevCookie [![Build status](https://ci.appveyor.com/api/projects/status/5bd2lcbrbv00g3mu?svg=true)](https://ci.appveyor.com/project/cottsak/devcookie)

> A helper library indended to enable rapid go-to-prod by providing early shared-key cookie based auth and Feature Toggling

When bulding MVPs, it's critical that infrastructure is not the focus. To this end there are a number of components that are very helpful in speeding up the go-to-prod process and DevCookie is one of them.

The goals of this package will be:

1. Providing simple infrastructure for a global authentication filter based on a shared-key "development cookie" which the team can use to introduce a thin security layer to a production website deployment, prior to full user management.

2. Using the same shared-key dev cookie, a simple interface is available to toggle features, page elements and other branching logic anywhere in the app.

These functions are critical to getting a MVP released quickly and should serve as one of the first packages a solution pulls in.

# So how do I use this?

1. [`Install-Package DevCookie`](https://www.nuget.org/packages/DevCookie/)

2. Register the Autofac module and specify your dev cookie secret key: `builder.RegisterModule(new DevAccessModule("U4SdMn12dTkLT4aktB75fvdpPcqnmEBc39aufs3QlGo6x2SZYo"));` (<< don't use this key!)

## I don't have any user authenticaton so I want to use DevCookie to protect my whole site

1. When registering the `DevAccessModule`, use the `useAsGlobalAuthFilter` flag: `builder.RegisterModule(new DevAccessModule("U4SdMn12dTkLT4aktB75fvdpPcqnmEBc39aufs3QlGo6x2SZYo", useAsGlobalAuthFilter: true));`

Now all requests should return 404. To access a page simply append `?devaccess=U4SdMn12dTkLT4aktB75fvdpPcqnmEBc39aufs3QlGo6x2SZYo` to the url in your browser to create the cookie.

## I just want to [feature toggle](http://stackoverflow.com/a/7707394/56145) certain behaviour at the action/controller level

1. Make sure the `useAsGlobalAuthFilter` flag is set to `false`.
2. Use the `[DevCookieAuthorize]` on only those actions/controllers you wish to prevent public access to. Only requests that include the dev cookie (or query string param) will be able to access those actions.
3. When the feature goes live, remove the `[DevCookieAuthorize]` and redeploy.

## Well I'm toggling at the controller level now but I need to show/hide things in certain views

1. You can use something like the `DevAccessChecker` from a [`BasePage`](https://github.com/cottsak/DevCookie/blob/master/DevCookie.Web/Views/BasePage.cs) in this fashion:
```
@if (DevAccessChecker.UserHasDevAccess())
{
    <p>Looks like you're DEV. You're invited to the @Html.ActionLink("secret section!", "SecretPage", "Other")</p>
}
```

## Sounds great, but how do I branch at some arbitrary point in my stack?

1. Inject the `IDevAccessChecker` into your abstraction like is shown in the [`FooService` example](https://github.com/cottsak/DevCookie/blob/master/DevCookie.Web/Controllers/OtherController.cs):
```
    class FooService : IFooService
    {
        private readonly IDevAccessChecker _devAccessChecker;

        public FooService(IDevAccessChecker devAccessChecker)
        {
            _devAccessChecker = devAccessChecker;
        }

        public string GetFoo()
        {
            if (_devAccessChecker.UserHasDevAccess())
                return "the foo is strong with this one!";

            return string.Empty;
        }
    }
```

Now you should be able to toggle features on and off easily giving only "dev access" to those who need to preview/accept/test them in production.

Also, you can test the [example project](https://github.com/cottsak/DevCookie/tree/master/DevCookie.Web), with the [above token](https://github.com/cottsak/DevCookie/blob/master/DevCookie.Web/App_Start/ContainerConfig.cs), as it is deployed to [devcookie.apphb.com](http://devcookie.apphb.com/). You'll need to navigate to a [restricted page](https://devcookie.apphb.com/other/page) to get the 404 where you can then use the query string approach as listed above to set up the cookie and gain access.

Any [feedback, suggestions](https://github.com/cottsak/DevCookie/issues/new) or [pull requests](https://github.com/cottsak/DevCookie/pulls) are really appreciated.



[@mattkocaj](https://twitter.com/mattkocaj)
