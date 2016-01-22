using System.Web.Mvc;

namespace DevCookie.Web.Controllers
{
    public class OtherController : BaseController
    {
        private readonly IFooService _fooService;

        public OtherController(IFooService fooService)
        {
            _fooService = fooService;
        }

        // GET: Other
        public ActionResult Index()
        {
            if (string.IsNullOrWhiteSpace(_fooService.GetFoo()))
            {
                return Content("you're not a DEV!");
            }

            return RedirectToAction("Page");
        }

        [DevAccessAuthorize]
        public ActionResult Page()
        {
            return View();
        }

        [DevAccessAuthorize]
        public ActionResult SecretPage()
        {
            return View();
        }
    }

    public interface IFooService
    {
        string GetFoo();
    }

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
}