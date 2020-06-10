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
        //Fix code for Test.
        //int iUserId = 1;
        //int iDocId = 6;
        //----------------//

        DocumentInfoRepo docInfoRepo = new DocumentInfoRepo();
        DocumentDetailRepo docDetRepo = new DocumentDetailRepo();
        // GET: SelectPage
        public ActionResult Index()
        {
            //    int DocId = 6;
            //    var data = docInfoRepo.Get(DocId);
            //    ViewBag.Id = data.DocId;
            //    ViewBag.FilePath = data.FileUrl;
            //    ViewBag.CreateBy = data.CreateBy;

            //var docInfo = docInfoRepo.Get(iDocId);
            //ViewBag.PDFPath = docInfo.FilePath;
            return View("SelectPage");
        }

        public ActionResult SelectPage(int id)
        {
            //ViewBag.Id = id;
            var data = docInfoRepo.Get(id);
            ViewBag.Id = data.DocId;
            ViewBag.FileUID = data.FileUID;
            ViewBag.FileName = data.FileName;
            ViewBag.FilePath = data.FilePath;
            ViewBag.FileUrl = data.FileUrl;
            ViewBag.CreateBy = data.CreateBy;

            //var docInfo = docInfoRepo.Get(iDocId);
            //ViewBag.PDFPath = docInfo.FilePath;
            return View("SelectPage");
        }
        //public ActionResult SelectPageNumber()
        //{
        //    string PageNumber = Request.Form["txtPage"];
        //    try
        //    {
        //        String sCreateDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        //        DocumentDetailModel doc = new DocumentDetailModel();
        //        doc.DocId = iDocId;
        //        doc.DocPageNo = "1";
        //        doc.PageType = "1";
        //        doc.CreateBy = "tanasitj";
        //        doc.CreateDate = sCreateDate;

        //        DocumentDetailRepo documentDetailRepo = new DocumentDetailRepo();
        //        documentDetailRepo.Add(doc);

        //        //return Json("OK");
        //    }
        //    catch (Exception ex) { };
        //    return Json("No files selected.");
        //}
        public JsonResult SaveSelectPage(DocumentDetailModel item)
        {
            var data = docDetRepo.Add(item);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}