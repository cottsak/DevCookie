# DevCookie [![Build status](https://ci.appveyor.com/api/projects/status/5bd2lcbrbv00g3mu?svg=true)](https://ci.appveyor.com/project/cottsak/devcookie)

> A helper library intended to enable rapid go-to-prod by providing simple Feature Toggling helpers and optional early shared-key cookie based authentication.

## So why do I need this?

[Feature Toggling](http://martinfowler.com/bliki/FeatureToggle.html) (or [Feature Flagging](http://stackoverflow.com/a/7707394/56145)) is a great tool you can use to get stuff to prod fast! #shipit

It goes hand in hand with concepts like [CI](https://en.wikipedia.org/wiki/Continuous_integration)/[CD](https://en.wikipedia.org/wiki/Continuous_delivery) and other best practices like [Automated Testing](https://github.com/cottsak/testingstrategyguidance/blob/master/testing-strategy.md), etc. One of the great benefits of [Mainline Development](https://www.thoughtworks.com/insights/blog/enabling-trunk-based-development-deployment-pipelines) ([GitHub](https://guides.github.com/introduction/flow/) [Flow](http://scottchacon.com/2011/08/31/github-flow.html)) is that everything is integrated all the time and ideally, it should be ready for production all the time too. That's where DevCookie comes in.

DevCookie is a simple infrastructure which enables you to hide/show features easily using various hooks so that, even if a story/epic/feature is not complete, it can go to production integrated. Devs/testers/anyone on your team can then test it, show it to stakeholders and ultimately get it accepted without it being visible to the wild. Once the Feature Toggle is lifted, it's live and thoroughly de-risked. [There's a great article on Fowler's site](https://martinfowler.com/articles/feature-toggles.html#ATogglingTale) which goes into detail about the different types of toggles, specifically [Release Toggles](https://martinfowler.com/articles/feature-toggles.html#ReleaseToggles), which is where DevCookie fits in.

DevCookie is about [getting to prod fast in a low risk manner](https://agileforleads.com/)! This applies to the enterprise where apps have longer story lifetimes; but it's also useful for MVP/prototypes too. DevCookie can be used to show/hide small parts of the site or act as a simple authentication wall for the whole web-app until launch.

## So how do I use this?

1. [`Install-Package DevCookie`](https://www.nuget.org/packages/DevCookie/)

2. Ensure that you're [using Autofac for MVC so you can register dependencies](http://docs.autofac.org/en/latest/integration/mvc.html#quick-start). Sorry, but for now we're taking a hard dependency on Autofac.

3. Register the Autofac module and specify your dev cookie secret key: `builder.RegisterModule(new DevAccessModule("U4SdMn12dTkLT4aktB75fvdpPcqnmEBc39aufs3QlGo6x2SZYo"));` (<< don't use this key!)

### I don't have any user authentication so I want to use DevCookie to protect my whole site

1. When registering the `DevAccessModule`, use the `useAsGlobalAuthFilter` flag: `builder.RegisterModule(new DevAccessModule("U4SdMn12dTkLT4aktB75fvdpPcqnmEBc39aufs3QlGo6x2SZYo", useAsGlobalAuthFilter: true));`

Now all requests should return 404. To access a page simply append `?devaccess=U4SdMn12dTkLT4aktB75fvdpPcqnmEBc39aufs3QlGo6x2SZYo` to the url in your browser to create the cookie.

### I just want to [feature toggle](http://stackoverflow.com/a/7707394/56145) certain behaviour at the action/controller level

1. Make sure the `useAsGlobalAuthFilter` flag is set to `false`.
2. Use the `[DevAccessAuthorize]` on only those actions/controllers you wish to prevent public access to. Only requests that include the dev cookie (or query string param) will be able to access those actions.
3. When the feature goes live, remove the `[DevAccessAuthorize]` and redeploy.

### Well I'm toggling at the controller level now but I need to show/hide things in certain views

1. You can use something like the `DevAccessChecker` from a [`BasePage`](https://github.com/cottsak/DevCookie/blob/master/DevCookie.Web/Views/BasePage.cs) in this fashion:
```csharp
@if (DevAccessChecker.UserHasDevAccess())
{
    <p>Looks like you're DEV. You're invited to the @Html.ActionLink("secret section!", "SecretPage", "Other")</p>
}
```

### Sounds great, but how do I branch at some arbitrary point in my stack?

1. Inject the `IDevAccessChecker` into your abstraction like is shown in the [`FooService` example](https://github.com/cottsak/DevCookie/blob/master/DevCookie.Web/Controllers/OtherController.cs):
```csharp
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
