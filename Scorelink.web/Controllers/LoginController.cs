using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scorelink.web.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SubmitLogin(string userId, string password)
        {
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Register()
        {
            return RedirectToAction("Register", "Home");
        }
    }
}