using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scorelink.web.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index()
        {
            return View("AdminUserList");
        }

        public ActionResult EditUserProfile()
        {
            return View("AdminEditUserProfile");
        }
    }
}