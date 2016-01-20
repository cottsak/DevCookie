using System.Web.Mvc;
using DevCookie.Web.Controllers;

namespace DevCookie.Web.Views
{
    public abstract class BasePage : BasePage<dynamic> { }

    public abstract class BasePage<T> : WebViewPage<T>
    {
        protected IDevAccessChecker DevAccessChecker
        { get { return ((BaseController)ViewContext.Controller).DevAccessChecker; } }
    }
}