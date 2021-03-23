using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scorelink.BO.Helper;
using Scorelink.BO.Repositories;
using Scorelink.MO.DataModel;

namespace Scorelink.web.Controllers
{
    public class CompanyController : Controller
    {
        CompanyRepo comRepo = new CompanyRepo();
        // GET: Company
        public ActionResult Index()
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
            return View("CompanyList");
        }

        public JsonResult GetCompanyList()
        {
            var com = comRepo.GetList().ToList();
            return Json(com, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveCompany(string mode, CompanyModel item)
        {
            try
            {
                var data = "";
                CompanyRepo compRepo = new CompanyRepo();
                if (mode == "A")
                {
                    if (compRepo.CheckCompanyDup(item.CompanyName))
                    {
                        //Duplicate Username
                        data = "CompanyDup";
                    }
                    else if (compRepo.CheckDomainDup(item.Domain))
                    {
                        data = "DomainDup";
                    }
                    else
                    {
                        data = compRepo.Add(item);

                        var com = compRepo.GetCompanyByName(item.CompanyName);

                        string srcFolder = Common.getConstTxt("Dict") + "Template";
                        string desFolder = Common.getConstTxt("Dict") + Common.GenZero(com.CompanyId.ToString(), 5);

                        //Copy Template Folder
                        foreach (string dirPath in Directory.GetDirectories(srcFolder, "*",
                            SearchOption.AllDirectories))
                            Directory.CreateDirectory(dirPath.Replace(srcFolder, desFolder));

                        //Copy All File
                        foreach (string newPath in Directory.GetFiles(srcFolder, "*.*",
                            SearchOption.AllDirectories))
                            System.IO.File.Copy(newPath, newPath.Replace(srcFolder, desFolder), true);
                    }
                }
                else
                {
                    data = compRepo.Update(item);
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

        public JsonResult GetCompanyDetail(CompanyModel item)
        {
            CompanyRepo compRepo = new CompanyRepo();
            var com = compRepo.GetCompanyById(item.CompanyId);
            return Json(com, JsonRequestBehavior.AllowGet);
        }
    }
}