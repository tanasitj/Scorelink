using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scorelink.web.Controllers
{
    public class ScanEditController : Controller
    {
        // GET: ScanEdit
        public ActionResult Index()
        {
            return View("ScanResult");
        }
    }
}