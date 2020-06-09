using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scorelink.MO.DataModel;
using System.IO;
//using OfficeOpenXml;

namespace Scorelink.web.Controllers
{
    public class PreviewPageController : Controller
    {
        // GET: PreviewPage

        public ActionResult Index()
        {
            //string xValue = Request.Form["txtPage"];
            //  var xfileName = txtPage;




            //ViewBag.Message = string.Format("Hello {0}.\\nCurrent Date and Time: {1}", name, DateTime.Now.ToString());
            //return View("PreViewPage");
            //ResultModel empobj = new ResultModel();
            string textboxValue = Request.Form["txtPage"];
            if (textboxValue != null)
            {
                //empobj.ScanEdit = CheckResult();
                ViewData["Message"] = "select";
            }
            else
            {
                ViewData["Message"] = "notselect";
                // empobj.ScanEdit = MergeRow();
            }
            //return View("PreviewPage", empobj);
            return View("PreviewPage");
        }
    }
}