using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scorelink.web.Controllers
{
    public class SelectPatternController : Controller
    {
        // GET: SelectPattern
        public ActionResult Index()
        {
            return View("SelectPatternMain");
        }
    }
}