using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scorelink.BO.Helper;
using Scorelink.BO.Repositories;
using Scorelink.MO.DataModel;

namespace Scorelink.web.Controllers
{
    public class UserController : Controller
    {
        UserRepo userRepo = new UserRepo();
        // GET: User
        public ActionResult Index()
        {
            checkOnline();
            return View("UserList");
        }

        public JsonResult GetUserList()
        {
            checkOnline();
            var data = userRepo.GetUserList().ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCompanyDD()
        {
            checkOnline();
            var data = userRepo.GetCompanyDD();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetUserDetail(UserModel item)
        {
            checkOnline();
            var data = userRepo.GetUserById(item.UserId);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveUser(UserModel item)
        {
            try
            {
                checkOnline();
                var data = "";
                UserRepo userRepo = new UserRepo();
                if (item.UserId.ToString() == "0")
                {
                    if (userRepo.CheckUserDup(item.Email))
                    {
                        //Duplicate Username
                        data = "UserDup";
                    }
                    else
                    {
                        item.UserName = item.Email;
                        item.Password = "P@ssw0rd";
                        data = userRepo.Add(item);
                    }
                }
                else
                {
                    data = userRepo.Update(item);
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger Err = new Logger();
                Err.ErrorLog(ex.ToString());
                return Json(ex.Message);
            }
        }

        private void checkOnline()
        {
            if (Session["UserId"] == null)
            {
                Response.Redirect("/Home/Index");
            }
            else
            {
                ViewBag.UserId = Session["UserId"].ToString();
                int iUserId = 0;
                Int32.TryParse(Session["UserId"].ToString(), out iUserId);

                //Get User Info.
                UserRepo userRepo = new UserRepo();
                var userDB = userRepo.Get(iUserId);
                ViewBag.Name = userDB.Name;
                ViewBag.Surname = userDB.Surname;

                //Check and Update online date time.
                OnlineUserRepo onlineRepo = new OnlineUserRepo();
                var online = onlineRepo.Get(iUserId);
                onlineRepo.Update(online);
            }
        }
    }
}