using System.Web.Mvc;

namespace DevCookie.Web.Controllers
{
    public class BaseController : Controller
    {
        public IDevAccessChecker DevAccessChecker { get; set; }
    }
}