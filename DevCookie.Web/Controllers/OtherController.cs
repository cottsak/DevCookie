using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DevCookie.Web.Controllers
{
    public class OtherController : Controller
    {
        // GET: Other
        public ActionResult Index()
        {
            return RedirectToAction("Page");
        }

        public ActionResult Page()
        {
            return View();
        }
    }
}