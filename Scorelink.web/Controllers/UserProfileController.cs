using Scorelink.BO.Repositories;
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

                int iUserId = 0;
                Int32.TryParse(Session["UserId"].ToString(), out iUserId);

                UserRepo userRepo = new UserRepo();
                var userDB = userRepo.Get(iUserId);

                CompanyRepo compRepo = new CompanyRepo();
                var compDB = compRepo.GetCompanyById(userDB.Company ?? 0);

                ViewBag.Name = userDB.Name;
                ViewBag.Surname = userDB.Surname;
                ViewBag.Email = userDB.Email;
                ViewBag.Company = userDB.Company;
                ViewBag.CompanyName = compDB.CompanyName;
                ViewBag.Address = userDB.Address;
                ViewBag.Telephone = userDB.Telephone;
                ViewBag.RegisterDate = userDB.RegisterDate;
                ViewBag.ExpireDate = userDB.ExpireDate;
            }

            return View("UserProfile");
        }
    }
}