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
        // GET: TestPage
        DocumentDetailRepo docDetailRepo = new DocumentDetailRepo();
        DocumentInfoRepo docInfoRepo = new DocumentInfoRepo();
        public ActionResult Index()
        {
            int DocId = 14;
            var data = docInfoRepo.Get(DocId);
            ViewBag.Id = data.DocId;
            ViewBag.FileUrl = data.FileUrl;
            ViewBag.CreateBy = data.CreateBy;

            var docInfo = docInfoRepo.Get(DocId);
            ViewBag.PDFPath = docInfo.FilePath;
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
            return View("SelectPage");
        }
        public ActionResult DeletePage(int id,string pagetype)
        {
            //Delete page of page seleted
            var data = docDetailRepo.Get(id);
            ViewBag.Id = data.DocId;
            ViewBag.PageType = data.PageType;
            return View("DeletePage");
        }
        public JsonResult Get_SelectPage(string value, string pageType, int docId, string docPageNo)
        {
            try
            {
                //Insert table DocumentDetail           
                DocumentDetailModel docDetail = new DocumentDetailModel();
                var data_detail = docInfoRepo.Get(docId);
                docDetail.DocId = docId;
                docDetail.DocPageNo = docPageNo;
                docDetail.PageType = pageType;
                docDetail.FootnoteNo = null;
                docDetail.ScanStatus = null;
                docDetail.PageFileName = data_detail.FileName;
                docDetail.PagePath = data_detail.FilePath;
                docDetail.Selected = null;
                docDetail.PatternNo = null;
                docDetail.CreateBy = data_detail.CreateBy;
                docDetail.PageUrl = data_detail.FileUrl;
                docDetail.CreateDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                docDetail.UpdateDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                DocumentDetailRepo docDetailRepo = new DocumentDetailRepo();
                docDetailRepo.Add(docDetail);
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

            //Return Next Page Data
            var data = docDetailRepo.Get(docId);
            if (data == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult GetDocumentList(int filterId)
        {
            var doc = docDetailRepo.GetListView(filterId).ToList();
            return Json(doc, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteDocumentDetail(string docid,string pagetype,string docPageNo)
        {
            var result = "";    
                try
                {
                        result = docDetailRepo.DeleteTypes(docid,pagetype, docPageNo);
                    
                }
                catch (Exception ex)
                {
                    return Json(ex.Message);
                }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}