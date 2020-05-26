using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scorelink.web.Controllers
{
    public class SelectPageController : Controller
    {
        // GET: SelectPage
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SelectPage(string id)
        {
            ViewBag.Id = id;
            return View();
        }
    }
}