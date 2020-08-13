using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Scorelink.web.Controllers
{
    public class UserProfileController : Controller
    {
        // GET: UserProfile
        public ActionResult Index()
        {
            //if(!string.IsNullOrEmpty(Session["UserId"].ToString()))
            if (Session["UserId"] == null)
            {
                Response.Redirect("/Home/Index");
            }
            else
            {
                ViewBag.UserId = Session["UserId"].ToString();
                ViewBag.Name = Session["Name"].ToString();
                ViewBag.Surname = Session["Surname"].ToString();
                ViewBag.Email = Session["Email"].ToString();
                ViewBag.Company = Session["Company"].ToString();
                ViewBag.Telephone = Session["Telephone"].ToString();
                ViewBag.RegisterDate = Session["RegisterDate"].ToString();
                ViewBag.ExpireDate = Session["ExpireDate"].ToString();
            }

            return View("UserProfile");
        }
    }
}