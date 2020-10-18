using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Scorelink.BO.Helper;
using Scorelink.MO.DataModel;
using Scorelink.BO.Repositories;
using System.IO;

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
                onlineRepo.CheckTimeOut(online);

                var data = docInfoRepo.Get(id);
                ViewBag.Id = data.DocId;
                ViewBag.FileUID = data.FileUID;
                ViewBag.FileName = data.FileName;
                ViewBag.FilePath = data.FilePath;
                ViewBag.FileUrl = data.FileUrl;
                ViewBag.CreateBy = data.CreateBy;
            }
            return View("SelectPage");
        }
        public ActionResult DeletePage(int id, string pagetype)
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
                docDetail.PageFileName = Common.GenZero(Convert.ToString(pageType), 5);
                docDetail.PagePath = data_detail.FilePath;
                docDetail.Selected = null;
                docDetail.PatternNo = null;
                docDetail.CreateBy = data_detail.CreateBy;
                docDetail.PageUrl = data_detail.FileUrl;
                docDetail.CreateDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
                docDetail.UpdateDate = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                string sChk = "";
                SelectPageRepo selRepo = new SelectPageRepo();
                if (selRepo.CheckDocPage(docDetail))
                {
                    sChk = "Dup";
                }
                else
                {
                    DocumentDetailRepo docDetailRepo = new DocumentDetailRepo();
                    docDetailRepo.Add(docDetail);
                    sChk = docPageNo;
                }
                return Json(sChk, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Logger Err = new Logger();
                Err.ErrorLog(ex.ToString());
                return Json(ex.Message);
            }
        }
        public JsonResult GetDocumentList(int filterId)
        {
            SelectPageRepo selRepo = new SelectPageRepo();

            var doc = selRepo.GetListView(filterId).ToList();
            return Json(doc, JsonRequestBehavior.AllowGet);
        }
        public JsonResult DeleteDocumentDetail(string docid, string pagetype, string docPageNo)
        {
            var result = "";
            try
            {
                result = docDetailRepo.DeleteTypes(docid, pagetype, docPageNo);

            }
            catch (Exception ex)
            {
                Logger Err = new Logger();
                Err.ErrorLog(ex.ToString());
                return Json(ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        public JsonResult SelectScan(int docId, string pageType)
        {
            var result = "";
            try
            {
                //Get DocumentInfo data.
                var docInfo = docInfoRepo.Get(docId);
                String sFolder = docInfo.FileUID;
                String sPath = docInfo.FilePath;
                //Folder for New File.
                String sTempFolder = Common.getConstTxt("SLUserFlie") + Common.GenZero(docInfo.CreateBy, 8) + "\\" + sFolder + "\\";
                //Check and Create Folder.
                Common.CreateDocFolder(sTempFolder);
                //Get PageType for File Name.
                String sSavePath = sTempFolder + "SL" + Common.GenZero(pageType, 5) + ".tif";
                //Check for Delete File for Initail.
                if (System.IO.File.Exists(sSavePath))
                {
                    System.IO.File.Delete(sSavePath);
                }
                //Get DocumentDetail data.
                var docDet = docDetailRepo.GetList(docId, pageType);
                //Get Leadtools License.
                Common.GetLicenseLeadTool();

                using (var documentConverter = new Leadtools.Document.Converter.DocumentConverter())
                {
                    var codecs = new Leadtools.Codecs.RasterCodecs();
                    codecs.Options.RasterizeDocument.Load.XResolution = 150;
                    codecs.Options.RasterizeDocument.Load.YResolution = 150;

                    Leadtools.Codecs.CodecsImageInfo info = codecs.GetInformation(sPath, true);

                    int iNum = 1;
                    int iRow = docDet.Count();

                    foreach (var doc in docDet)
                    {
                        //Get Page Number convert to integer.
                        int iPage = Convert.ToInt32(doc.DocPageNo);
                        Leadtools.RasterImage image = codecs.Load(sPath, 0, Leadtools.Codecs.CodecsLoadByteOrder.BgrOrGray, iPage, iPage);
                        //Generate File.
                        codecs.Save(image, sSavePath, Leadtools.RasterImageFormat.Tif, 24, 1, -1, 1, Leadtools.Codecs.CodecsSavePageMode.Append);
                        //Check for Clear Temp.
                        if (iNum == iRow)
                        {
                            image.Dispose();
                        }
                        else
                        {
                            iNum++;
                        }
                    }

                    codecs.Dispose();
                    documentConverter.Dispose();
                    GC.Collect(0);
                }

                //Update Path to DocumentDetail.
                DocumentDetailModel docDetail = new DocumentDetailModel();
                docDetail.DocId = docId;
                docDetail.PageType = pageType;
                docDetail.ScanStatus = "";
                docDetail.PagePath = sTempFolder + "SL" + Common.GenZero(pageType, 5) + ".tif";
                docDetail.PageUrl = Common.getConstTxt("sUrl") + "/FileUploads/" + Common.GenZero(docInfo.CreateBy, 8) + "/" + sFolder + "/" + "SL" + Common.GenZero(pageType, 5) + ".tif";
                SelectPageRepo pageRepo = new SelectPageRepo();
                result = pageRepo.UpdatePathFile(docDetail);
            }
            catch (Exception ex)
            {
                Logger Err = new Logger();
                Err.ErrorLog(ex.ToString());
                return Json(ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}