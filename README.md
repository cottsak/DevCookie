# DevCookie
> A helper library indended to enable rapid go-to-prod by providing early shared-key cookie based auth and Feature Toggling

When bulding MVPs, it's critical that infrastructure is not the focus. To this end there are a number of components that are very helpful in speeding up the go-to-prod process and DevCookie is one of them.

The goals of this package will be:

1. Providing simple infrastructure for a global authentication filter based on a shared-key "development cookie" which the team can use to introduce a thin security layer to a production website deployment, prior to full user management.

2. Using the same shared-key dev cookie, a simple interface is available to toggle features, page elements and other branching logic anywhere in the app.

These functions are critical to getting a MVP released quickly and should serve as one of the first packages a solution pulls in.

# Up and running

TODO

[@mattkocaj](https://twitter.com/mattkocaj)
