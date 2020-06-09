using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scorelink.BO.Helper;
using Scorelink.MO.DataModel;
using Scorelink.BO.Repositories;

namespace Scorelink.web.Controllers
{
    public class SelectPageController : Controller
    {

        DocumentInfoRepo docInfoRepo = new DocumentInfoRepo();
        DocumentDetailRepo docDetRepo = new DocumentDetailRepo();
        // GET: SelectPage
        public ActionResult Index()
        {

            return View("SelectPage");
        }

        public ActionResult SelectPage(string id)
        {
            ViewBag.Id = id;
            //var data = docDetRepo.Get(id);
            //ViewBag.Id = data.DocId;
            //ViewBag.DocDetId = data.DocDetId;
            //ViewBag.DocPageNo = data.DocPageNo;
            //ViewBag.PageFileName = data.PageFileName;
            //ViewBag.PagePath = data.PagePath;

            //var docInfo = docInfoRepo.Get(iDocId);
            //ViewBag.PDFPath = docInfo.FilePath;
            return View("SelectPage");
        }

    }
}