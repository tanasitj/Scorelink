using System.Web.Mvc;
using Scorelink.MO.DataModel;

namespace Scorelink.web.Controllers
{
    public class HomeController : Controller
    {


        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AnotherLink()
        {
            return View("Index");
        }

        public ActionResult Register()
        {
            return View("Register");
        }

        public ActionResult Login()
        {
            return View("Login");
        }

        public ActionResult ForgotPassword()
        {
            return View("ForgotPassword");
        }

        public JsonResult SaveRegister(UserModel item)
        {
            var data = "";
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}
